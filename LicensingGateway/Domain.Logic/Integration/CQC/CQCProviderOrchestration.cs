using System.Net;
using Database.Entites;
using Database.LicenceHolder.Readonly.Repository;
using Database.Repository.CQC;
using Domain.Logic.Integration.CQC.API;
using Domain.Logic.Integration.CQC.API.Models;
using Domain.Logic.Integration.CQC.Factories;
using Domain.Logic.Integration.CQC.Models;
using Domain.Models.Exceptions;
using Microsoft.Extensions.Logging;

namespace Domain.Logic.Integration.CQC;
public class CQCProviderOrchestration(
    ICQCApiWrapper cqcApiWrapper,
    IRepositoryForCQCProvider repositoryForCQCProvider,
    IRepositoryForCQCProviderImportPage repositoryForCQCProviderImportPageProviderId,
    IRespositoryForCQCProviderImportInstance respositoryForCQCProviderImportInstance,
    ILogger<CQCProviderOrchestration> logger,
    ICQCAddressFactory addressFactory,
    ILicenceHolderRespositoryForLicence licenceHolderRespositoryForLicence) : ICQCProviderOrchestration
{
    private readonly ICQCApiWrapper cqcApiWrapper = cqcApiWrapper;
    private readonly IRepositoryForCQCProvider repositoryForCQCProvider = repositoryForCQCProvider;
    private readonly IRepositoryForCQCProviderImportPage repositoryForCQCProviderImportPageProviderId = repositoryForCQCProviderImportPageProviderId;
    private readonly IRespositoryForCQCProviderImportInstance respositoryForCQCProviderImportInstance = respositoryForCQCProviderImportInstance;
    private readonly ILogger logger = logger;
    private readonly ICQCAddressFactory addressFactory = addressFactory;
    private readonly ILicenceHolderRespositoryForLicence licenceHolderRespositoryForLicence = licenceHolderRespositoryForLicence;

    public async Task<ProvidersOutputModel> CQCGetAllProviders(ProvidersInputModel inputModel)
    {
        var result = await cqcApiWrapper.GetProviders(inputModel.PageNumber);

        if (result.isSuccess && result.model != null)
        {
            foreach (var provider in result.model.providers)
            {
                await repositoryForCQCProviderImportPageProviderId.AddAsync(
                    inputModel.InstanceId,
                    inputModel.PageNumber,
                    provider.providerId);
            }

            return result.model;
        }

        throw new CQCAPIException($"Failed to obtain providers for instance: {inputModel.InstanceId}, page number: {inputModel.PageNumber} ,result: {result.isSuccess}");
    }

    public async Task CreateInstanceRecord(Guid guid)
    {
        await respositoryForCQCProviderImportInstance.AddAsync(new Database.Entites.CQCProviderImportInstance() { Id = guid });
    }

    public async Task DeleteInstanceRecord(Guid guid)
    {
        await respositoryForCQCProviderImportInstance.DeleteAsync(guid);
    }

    public async Task<string[]> GetProviderIdsForThisImport(ProvidersInputModel inputModel)
    {
        return await repositoryForCQCProviderImportPageProviderId.GetProviderIdsForImport(
            inputModel.InstanceId,
            inputModel.PageNumber);
    }

    public async Task<CQCProviderInformation?> GetProviderInformation(string id)
    {
        var databaseId = await repositoryForCQCProvider.GetIdIfExistsAsync(id);

        if (databaseId != null)
        {
            var entity = await repositoryForCQCProvider.GetByIdAsync((int)databaseId) ?? throw new Exception("Entity not found");

            return new CQCProviderInformation
            {
                Name = entity.Name,
                Address = addressFactory.Create(
                    entity.Address_Line_1,
                    entity.Address_Line_2,
                    entity.TownCity,
                    entity.Region,
                    entity.Postcode),
                PhoneNumber = entity.PhoneNumber ?? string.Empty,
                HasLicence = await licenceHolderRespositoryForLicence.HasActiveLicence(id)
            };
        }

        return null;
    }

    public async Task<ProvidersThatHaveChangedOutputModel> GetProvidersThatHaveChanged(ProvidersInputModel inputModel)
    {
        var result = await cqcApiWrapper.GetProvidersThatHaveChanged(inputModel.PageNumber);

        if (result.isSuccess && result.model != null)
        {
            foreach (var provider in result.model.changes)
            {
                await repositoryForCQCProviderImportPageProviderId.AddAsync(
                    inputModel.InstanceId,
                    inputModel.PageNumber,
                    provider);
            }

            return result.model;
        }

        logger.LogError("Failed to get providers that have changed: {isSuccess}, {inputModel.PageNumber}", result.isSuccess, inputModel.PageNumber);

        throw new CQCAPIException($"Failed to get providers that have changed: {result.isSuccess}, {inputModel.PageNumber}");
    }

    public async Task<string[]> GetProvidersRequlatedActivites(string cqcProviderId)
    {
        var id = await repositoryForCQCProvider.GetIdIfExistsAsync(cqcProviderId) ?? throw new NotFoundException($"CQC provider not found: {cqcProviderId}");

        return await repositoryForCQCProvider.GetProvidersRegulatedActivities(id);
    }

    public async Task ImportProvider(ProviderImportInputModel inputModel)
    {
        var update = false;

        try
        {
            var result = await cqcApiWrapper.GetProviderAsync(inputModel.ProviderID);

            if (result.isSuccess && result.model != null)
            {
                var databaseId = await repositoryForCQCProvider.GetIdIfExistsAsync(inputModel.ProviderID);

                if (databaseId == null)
                {
                    if(result.model.registrationStatus == "Registered")
                    {
                        var provider = new Database.Entites.CQCProvider
                        {
                            Name = result.model.name,
                            CQCProviderID = result.model.providerId,
                            Address_Line_1 = result.model.postalAddressLine1,
                            Address_Line_2 = result.model.postalAddressLine2,
                            TownCity = result.model.postalAddressTownCity,
                            Region = result.model.region,
                            Postcode = result.model.postalCode,
                            PhoneNumber = result.model.mainPhoneNumber,
                            WebsiteURL = result.model.website,
                            CQCProviderToRegulatedActivities = []
                        };

                        await CreateActivities(result.model.regulatedActivities, provider);

                        await repositoryForCQCProvider.AddAsync(provider);
                    }
                }
                else
                {
                    var cqcProviderId = (int)databaseId;

                    await repositoryForCQCProvider.DeleteProvidersRegulatedActivites(cqcProviderId);

                    var entity = await repositoryForCQCProvider.GetByIdAsync(cqcProviderId) ?? throw new NotFoundException("Entity not found");

                    if(result.model.registrationStatus == "Registered")
                    {
                        entity.Name = result.model.name;
                        entity.Address_Line_1 = result.model.postalAddressLine1;
                        entity.Address_Line_2 = result.model.postalAddressLine2;
                        entity.TownCity = result.model.postalAddressTownCity;
                        entity.Region = result.model.region;
                        entity.Postcode = result.model.postalCode;
                        entity.PhoneNumber = result.model.mainPhoneNumber;
                        entity.WebsiteURL = result.model.website;

                        await CreateActivities(result.model.regulatedActivities, entity);

                        await repositoryForCQCProvider.SaveChangesAsync();
                    }
                    else
                    {
                        await repositoryForCQCProvider.DeleteAsync(entity.Id);
                    }
                }

                await repositoryForCQCProviderImportPageProviderId.Update(200, inputModel.ProviderID, inputModel.InstanceId);

                update = true;

                return;
            }

            switch (result.httpStatusCode)
            {
                    case HttpStatusCode.NotFound:
                        var databaseId = await repositoryForCQCProvider.GetIdIfExistsAsync(inputModel.ProviderID);

                        if (databaseId != null)
                        {
                            logger.LogInformation("Provider {id} found in CQC API, so deleting from database", inputModel.ProviderID);

                            await repositoryForCQCProvider.DeleteAsync((int)databaseId);
                        }

                        await repositoryForCQCProviderImportPageProviderId.Update((int)result.httpStatusCode, inputModel.ProviderID, inputModel.InstanceId);
                        update = true;

                    break;

                default:
                    var exception = new CQCImportProviderOrchestrationException($"Failed to import provider {inputModel.ProviderID}, status code: {result.httpStatusCode}");

                    logger.LogError(exception, "Failed to import provider {id}, status code: {statusCode}", inputModel.ProviderID, result.httpStatusCode);

                    await repositoryForCQCProviderImportPageProviderId.Update((int)result.httpStatusCode, inputModel.ProviderID, inputModel.InstanceId);
                    update = true;

                    throw exception;
            }          
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to persist provider {id}, instance {instance}", inputModel.ProviderID, inputModel.InstanceId);

            if(update == false)
            {
                await repositoryForCQCProviderImportPageProviderId.Update(500, inputModel.ProviderID, inputModel.InstanceId);
            }

            throw;
        }
    }

    private async Task CreateActivities(RegulatedActivities[] regulatedActivities, CQCProvider provider)
    {
        foreach (var activity in regulatedActivities)
        {
            var regulatedActivity = await repositoryForCQCProvider.AquireActivity(activity.code, activity.name);

            provider.CQCProviderToRegulatedActivities.Add(new CQCProviderToRegulatedActivities
            {
                CQCProvider = provider,
                Activity = regulatedActivity
            });
        }
    }
}
