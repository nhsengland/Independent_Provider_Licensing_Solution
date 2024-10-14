using System.Text;
using Database.Entites;
using Database.LicenceHolder.Readonly.Repository;
using Database.Repository.Application;
using Database.Repository.ApplicationCode;
using Database.Repository.CQC;
using Database.Repository.Email;
using Database.Repository.UltimateController;
using Domain.Logic.Forms.Application.Models;
using Domain.Logic.Forms.Application.Page;
using Domain.Logic.Forms.Application.Rules;
using Domain.Logic.Forms.Factories;
using Domain.Logic.Forms.Helpers;
using Domain.Logic.Forms.Helpers.Session;
using Domain.Logic.Integration.CQC.Factories;
using Domain.Logic.Integration.StorageAccount.Queues;
using Domain.Models.Database.DTO;
using Domain.Models.Forms.Application;
using Domain.Models.Forms.Rules;
using Domain.Models.ViewModels.Application;

namespace Domain.Logic.Forms.Application;
public class ApplicationOrchestration(
    IRepositoryForApplication repositoryForApplication,
    IRepositoryForCQCProvider repositoryForCQCProvider,
    ICQCAddressFactory cQCAddressFactory,
    IReferenceIDFactory referenceIDFactory,
    IDirectorOrchestration directorOrchestration,
    IRepositoryForEmailNotifications repositoryForEmailNotifications,
    IStorageAccountQueueWrapper storageAccountQueueWrapper,
    IRespositoryForApplicationCode repositoryForApplicationCode,
    IRepositoryForPreApplication repositoryForPreApplication,
    IDateEvaluation dateEvaluation,
    IResponseConverter responseConverter,
    IRepositoryForUltimateController repositoryForUltimateController,
    INextPageOrchestor nextPageOrchestor,
    IPageToControllerMapper pageToControllerMapper,
    IDateTimeFactory dateTimeFactory,
    ISessionDateHandler sessionDateHandler,
    IApplicationBusinessRules applicationBusinessRules,
    ILicenceHolderRespositoryForLicence licenceHolderRespositoryForLicence) : IApplicationOrchestration
{
    private readonly IRepositoryForApplication repositoryForApplication = repositoryForApplication;
    private readonly IRepositoryForCQCProvider repositoryForCQCProvider = repositoryForCQCProvider;
    private readonly ICQCAddressFactory cQCAddressFactory = cQCAddressFactory;
    private readonly IReferenceIDFactory referenceIDFactory = referenceIDFactory;
    private readonly IDirectorOrchestration directorOrchestration = directorOrchestration;
    private readonly IRepositoryForEmailNotifications repositoryForEmailNotifications = repositoryForEmailNotifications;
    private readonly IStorageAccountQueueWrapper storageAccountQueueWrapper = storageAccountQueueWrapper;
    private readonly IRespositoryForApplicationCode repositoryForApplicationCode = repositoryForApplicationCode;
    private readonly IRepositoryForPreApplication repositoryForPreApplication = repositoryForPreApplication;
    private readonly IDateEvaluation dateEvaluation = dateEvaluation;
    private readonly IResponseConverter responseConverter = responseConverter;
    private readonly IRepositoryForUltimateController repositoryForUltimateController = repositoryForUltimateController;
    private readonly INextPageOrchestor nextPageOrchestor = nextPageOrchestor;
    private readonly IPageToControllerMapper pageToControllerMapper = pageToControllerMapper;
    private readonly IDateTimeFactory dateTimeFactory = dateTimeFactory;
    private readonly ISessionDateHandler sessionDateHandler = sessionDateHandler;
    private readonly IApplicationBusinessRules applicationBusinessRules = applicationBusinessRules;
    private readonly ILicenceHolderRespositoryForLicence licenceHolderRespositoryForLicence = licenceHolderRespositoryForLicence;

    public Task<int> CreateUltimateContorller(int applicationId)
    {
        return repositoryForUltimateController.Create(applicationId);
    }

    public Task DeleteUltimateContorller(int applicationId, int ultimateControllerId)
    {
        return repositoryForUltimateController.Delete(applicationId, ultimateControllerId);
    }

    public DateOnly? EvaluateDate(string date)
    {
        return dateEvaluation.EvaluateDate(date);
    }

    public RuleOutcomeDTO EvaluateLastFinancialYearEnd(
        int? day,
        int? month,
        int? year)
    {
        var firstChecks = applicationBusinessRules.EvaluateDate(day, month, year);

        if (!firstChecks.IsSuccess)
        {
            return firstChecks;
        }

        if (!day.HasValue || !month.HasValue || !year.HasValue)
        {
            throw new NotImplementedException("Unable to cast to Date");
        }

        return applicationBusinessRules.IsLastFinancialYearEndDateValid(new DateOnly(year.Value, month.Value, day.Value));
    }

    public RuleOutcomeDTO EvaluateNextFinancialYearEnd(
        int? day,
        int? month,
        int? year)
    {
        var firstChecks = applicationBusinessRules.EvaluateDate(day, month, year);

        if (!firstChecks.IsSuccess)
        {
            return firstChecks;
        }

        if (!day.HasValue || !month.HasValue || !year.HasValue)
        {
            throw new NotImplementedException("Unable to cast to Date");
        }

        return applicationBusinessRules.IsNextFinancialYearEndDateValid(new DateOnly(year.Value, month.Value, day.Value));
    }

    public async Task<(bool providerExists, bool providerHasActiveLicence)> OrchestrateCQCProvider(
        int id,
        string cqcProviderId)
    {
        await SetCQCProviderID(id, cqcProviderId);

        var (exists, providerDetails) = await GetCQCProviderDetailsIfExists(cqcProviderId);

        if (exists)
        {
            await SetCQCProviderDetails(id, new()
            {
                Name = providerDetails.Name,
                Address = providerDetails.Address,
                PhoneNumber = providerDetails.PhoneNumber,
                WebsiteURL = providerDetails.WebsiteURL
            });
        }

        return (exists, await licenceHolderRespositoryForLicence.HasActiveLicence(cqcProviderId));
    }

    public async Task<RuleOutcomeDTO> OrchestrateLastFinancialYearEndAsync(
        int id,
        ApplicationDateDTO input)
    {
        sessionDateHandler.SetDay(input.Day);
        sessionDateHandler.SetMonth(input.Month);
        sessionDateHandler.SetYear(input.Year);
        sessionDateHandler.UseSessionValues();

        var outcome = EvaluateLastFinancialYearEnd(input.Day, input.Month, input.Year);

        if (outcome.IsSuccess)
        {
            await SetLastFinancialYear(id, sessionDateHandler.GetDate());

            sessionDateHandler.Reset();
        }
        else
        {
            await repositoryForApplication.SetLastFinancialYear(id, null);
        }

        return outcome;
    }

    public async Task<RuleOutcomeDTO> OrchestrateNextFinancialYearEndAsync(
        int id,
        ApplicationDateDTO input)
    {
        sessionDateHandler.SetDay(input.Day);
        sessionDateHandler.SetMonth(input.Month);
        sessionDateHandler.SetYear(input.Year);
        sessionDateHandler.UseSessionValues();

        var outcome = EvaluateNextFinancialYearEnd(input.Day, input.Month, input.Year);

        if (outcome.IsSuccess)
        {
            await SetNextFinancialYear(id, sessionDateHandler.GetDate());

            sessionDateHandler.Reset();
        }
        else
        {
            await repositoryForApplication.SetNextFinancialYear(id, null);
        }

        return outcome;
    }

    public Task<List<UltimateControllerDTO>> GetUltimateContorllers(int applicationId)
    {
        return repositoryForUltimateController.GetAll(applicationId);
    }

    public Task<string> GetApplicationCode(int id)
    {
        return repositoryForApplication.GetApplicationCode(id);
    }

    public async Task<ApplicationReviewData> GetApplicationReviewData(int id)
    {
        var responses = await GetApplicationResponses(id);

        return new ApplicationReviewData()
        {
            Responses = responses,
            RuleOutcomeDTO = applicationBusinessRules.AllowedToSubmitApplication(responses)
        };
    }

    public async Task<ReviewApplicationResponsesViewModel> GetApplicationResponses(int id)
    {
        var result = new ReviewApplicationResponsesViewModel();

        var application = await GetApplicationDTO(id);

        result.CompanyDetails.Add(new()
        {
            Question = "CQC Provider ID",
            Response = application.CQCProviderID,
            Page = Domain.Models.Database.ApplicationPage.CQCProviderID,
            IsComplete = !string.IsNullOrWhiteSpace(application.CQCProviderID)
        });

        result.CompanyDetails.Add(new()
        {
            Question = "Organisation Details",
            Response = CreateResponseForCQCProviderDetails(application),
            Page = Domain.Models.Database.ApplicationPage.CQCProviderChange,
            IsComplete = !string.IsNullOrWhiteSpace(application.CQCProviderName) && !string.IsNullOrWhiteSpace(application.CQCProviderAddress)
        });

        result.CompanyDetails.Add(new()
        {
            Question = "Do you have a Company number",
            Response = responseConverter.ConvertToYesOrNo(application.CompanyNumberCheck),
            Page = Domain.Models.Database.ApplicationPage.CompanyNumberCheck,
            IsComplete = application.CompanyNumberCheck != null
        });

        if (application.CompanyNumberCheck == true)
        {
            result.CompanyDetails.Add(new()
            {
                Question = "Company number",
                Response = application.CompanyNumber,
                Page = Domain.Models.Database.ApplicationPage.CompanyNumber,
                IsComplete = !string.IsNullOrWhiteSpace(application.CompanyNumber)
            });
        }

        result.CompanyDetails.Add(new()
        {
            Question = "Hasn’t had a financial period end yet?",
            Response = responseConverter.ConvertToYesOrNo(application.NewlyIncorporatedCompany),
            Page = Domain.Models.Database.ApplicationPage.NewlyIncorporatedCompany,
            IsComplete = application.NewlyIncorporatedCompany != null
        });

        if (application.NewlyIncorporatedCompany != null && application.NewlyIncorporatedCompany == false)
        {
            result.CompanyDetails.Add(new()
            {
                Question = $"Last financial year end for {application.CQCProviderName}",
                Response = application.LastFinancialYear?.ToString("dd-MM-yyyy") ?? string.Empty,
                IsDate = true,
                FinancialYear = application.LastFinancialYear ?? DateOnly.MinValue,
                Page = Domain.Models.Database.ApplicationPage.LastFinancialYear,
                IsComplete = application.LastFinancialYear != null
            });
            
        }

        result.CompanyDetails.Add(new()
        {
            Question = $"Next financial year end for {application.CQCProviderName}",
            Response = application.NextFinancialYear?.ToString("dd-MM-yyyy") ?? string.Empty,
            IsDate = true,
            FinancialYear = application.NextFinancialYear ?? DateOnly.MinValue,
            Page = Domain.Models.Database.ApplicationPage.NextFinancialYear,
            IsComplete = application.NextFinancialYear != null
        });

        foreach (var detail in result.CompanyDetails)
        {
            detail.Controller = pageToControllerMapper.Map(detail.Page);
        }

        result.DirectorCheck = new()
        {
            Question = "Do you have directors",
            Response = await GetDirectorsCheck(id),
            Page = Domain.Models.Database.ApplicationPage.DirectorCheck,
            IsComplete = application.DirectorsCheck != null,
            Controller = pageToControllerMapper.Map(Domain.Models.Database.ApplicationPage.DirectorCheck)
        };

        result.Directors = await directorOrchestration.GetDirectors(id, Domain.Models.Database.DirectorType.Board);

        result.CorporateDirectorCheck = new()
        {
            Question = "Do you have corporate Directors",
            Response = responseConverter.ConvertToYesOrNo(application.CorporateDirectorsCheck),
            Page = Domain.Models.Database.ApplicationPage.CorporateDirectorCheck,
            IsComplete = application.CorporateDirectorsCheck != null,
            Controller = pageToControllerMapper.Map(Domain.Models.Database.ApplicationPage.CorporateDirectorCheck)
        };

        result.CorporateDirectorGroups = await directorOrchestration.GetGroups(id, Domain.Models.Database.DirectorType.Corporate);

        foreach (var group in result.CorporateDirectorGroups)
        {
            group.Directors = await directorOrchestration.GetDirectors(id, Domain.Models.Database.DirectorType.Corporate, group.Id);
        }

        result.ShowParentCompanyDirectorSection = await directorOrchestration.CountDirectorsOfGroupType(
            id,
            Domain.Models.Database.DirectorType.Corporate) == 0;

        result.ParentCompanyDirectorCheck = new()
        {
            Question = "Do you have one or more parent companies",
            Response = responseConverter.ConvertToYesOrNo(application.OneOrMoreParentCompanies),
            Page = Domain.Models.Database.ApplicationPage.ParentCompaniesCheck,
            IsComplete = application.OneOrMoreParentCompanies != null,
            Controller = pageToControllerMapper.Map(Domain.Models.Database.ApplicationPage.ParentCompaniesCheck)
        };

        result.ParentCompanyGroups = await directorOrchestration.GetGroups(id, Domain.Models.Database.DirectorType.ParentCompany);

        foreach (var group in result.ParentCompanyGroups)
        {
            group.Directors = await directorOrchestration.GetDirectors(id, Domain.Models.Database.DirectorType.ParentCompany, group.Id);
        }

        result.FinalChecks.Add(new()
        {
            Question = "G3: Fit and proper persons as Directors",
            Response = responseConverter.ConvertToYesOrNo(application.DirectorsSatisfyG3FitAndProperRequirements),
            Page = Domain.Models.Database.ApplicationPage.DirectorsSatisfyG3FitAndProperRequirements,
            IsComplete = application.DirectorsSatisfyG3FitAndProperRequirements != null
        });

        if (!string.IsNullOrWhiteSpace(application.DirectorsSatisfyG3FitAndProperRequirements_IfNoWhy))
        {
            result.FinalChecks.Add(new()
            {
                Question = "If no why",
                Response = application.DirectorsSatisfyG3FitAndProperRequirements_IfNoWhy,
                Page = Domain.Models.Database.ApplicationPage.DirectorsSatisfyG3FitAndProperRequirements,
                IsComplete = true
            });
        }

        result.IsCrsOrHardToReplace = await IsCrsOrHardToReplace(id);

        if (result.IsCrsOrHardToReplace)
        {
            result.UltimateControllerCheck = new()
            {
                Question = "Do you have an ultimate controller",
                Response = responseConverter.ConvertToYesOrNo(application.UltimateController),
                Page = Domain.Models.Database.ApplicationPage.UltimateController,
                IsComplete = application.UltimateController != null,
                Controller = pageToControllerMapper.Map(Domain.Models.Database.ApplicationPage.UltimateController)
            };

            result.UltimateControllers = await GetUltimateContorllers(id);
        }

        foreach (var detail in result.FinalChecks)
        {
            detail.Controller = pageToControllerMapper.Map(detail.Page);
        }

        nextPageOrchestor.EvaluateApplicationResponses(result);

        if (string.IsNullOrWhiteSpace(result.Action))
        {
            var page = await GetCurrentPage(id);
            result.Controller = pageToControllerMapper.Map(page);
            result.Action = page.ToString();
        }

        return result;
    }

    public Task<ApplicationDTO> GetApplicationDTO(int id)
    {
        return repositoryForApplication.GetApplicationDTO(id);
    }

    public Task<string> GetCompanyNumber(int id)
    {
        return repositoryForApplication.GetCompanyNumber(id);
    }

    public async Task<string> GetContactEmailAddress(int id)
    {
        var contactDetails = await repositoryForApplication.GetContactDetails(id);

        return contactDetails.Email;
    }

    public async Task<string> GetCompanyNumberCheck(int id)
    {
        var value = await repositoryForApplication.GetCompanyNumberCheck(id);

        return responseConverter.ConvertToYesOrNo(value);
    }

    public async Task<string> GetNewlyIncorporatedCompany(int id)
    {
        var value = await repositoryForApplication.GetNewlyIncorporatedCompany(id);

        return responseConverter.ConvertToYesOrNo(value);
    }

    public async Task<string> GetDirectorsCheck(int id)
    {
        var value = await repositoryForApplication.GetDirectorsCheck(id);

        return responseConverter.ConvertToYesOrNo(value);
    }

    public async Task<string> GetCorporateDirectorsCheck(int id)
    {
        var value = await repositoryForApplication.GetCorporateDirectorsCheck(id);

        return responseConverter.ConvertToYesOrNo(value);
    }

    public Task<string> GetCQCProviderAddress(int id)
    {
        return repositoryForApplication.GetCQCProviderAddress(id);
    }

    public async Task<CQCProviderDetailsDTO> GetCQCProviderDetails(int id)
    {
        return await repositoryForApplication.GetCQCProviderDetails(id);
    }

    public async Task<(bool exists, CQCProviderDetailsDTO providerDetails)> GetCQCProviderDetailsIfExists(string id)
    {
        var cqcProvideriD = await repositoryForCQCProvider.GetIdIfExistsAsync(id);

        if (cqcProvideriD != null)
        {
            var provider = await repositoryForCQCProvider.GetByIdAsync(cqcProvideriD.Value);

            if (provider != null)
            {
                return (true, new CQCProviderDetailsDTO()
                {
                    Id = id,
                    Name = provider.Name,
                    Address = cQCAddressFactory.Create(
                        provider.Address_Line_1,
                        provider.Address_Line_2,
                        provider.TownCity,
                        provider.Region,
                        provider.Postcode),
                    PhoneNumber = provider.PhoneNumber ?? string.Empty,
                    WebsiteURL = provider.WebsiteURL ?? string.Empty
                });
            }
        }

        return (false, new CQCProviderDetailsDTO());
    }

    public Task<string> GetCQCProviderID(int id)
    {
        return repositoryForApplication.GetCQCProviderID(id);
    }

    public Task<string> GetCQCProviderName(int id)
    {
        return repositoryForApplication.GetCQCProviderName(id);
    }

    public Task<string> GetCQCProviderPhoneNumber(int id)
    {
        return repositoryForApplication.GetCQCProviderPhoneNumber(id);
    }

    public Task<string> GetCQCProviderWebsiteURL(int id)
    {
        return repositoryForApplication.GetCQCProviderWebsiteURL(id);
    }    

    public async Task<string> GetDirectorsSatisfyG3FitAndProperRequirements(int id)
    {
        var value = await repositoryForApplication.GetDirectorsSatisfyG3FitAndProperRequirements(id);

        return responseConverter.ConvertToYesOrNo(value);
    }

    public async Task<string> GetDirectorsSatisfyG3FitAndProperRequirementsIfNoWhy(int id)
    {
        return await repositoryForApplication.GetDirectorsSatisfyG3FitAndProperRequirementsIfNoWhy(id);
    }

    public async Task<ApplicationDateDTO> GetFinancialYearEndLastFromSessionOrDatabase(int id)
    {
        var hasValue = sessionDateHandler.HasValue();

        if (!hasValue)
        {
            sessionDateHandler.Set(await repositoryForApplication.GetFinancialYearEndLast(id));
        }

        var model = new ApplicationDateDTO()
        {
            Day = sessionDateHandler.GetDay(),
            Month = sessionDateHandler.GetMonth(),
            Year = sessionDateHandler.GetYear(),
            IsValidDate = sessionDateHandler.IsValidDate()
        };

        sessionDateHandler.Reset();

        return model;
    }

    public async Task<ApplicationDateDTO> GetFinancialYearEndNextFromSessionOrDatabase(int id)
    {
        var hasValue = sessionDateHandler.HasValue();

        if (!hasValue)
        {
            sessionDateHandler.Set(await repositoryForApplication.GetFinancialYearEndNext(id));
        }

        var model = new ApplicationDateDTO()
        {
            Day = sessionDateHandler.GetDay(),
            Month = sessionDateHandler.GetMonth(),
            Year = sessionDateHandler.GetYear(),
            IsValidDate = sessionDateHandler.IsValidDate()
        };

        sessionDateHandler.Reset();

        return model;
    }

    public async Task<string> GetOneOrMoreParentCompanies(int id)
    {
        return responseConverter.ConvertToYesOrNo(await repositoryForApplication.GetOneOrMoreParentCompanies(id));
    }

    public Task<string> GetReferenceId(int id)
    {
        return repositoryForApplication.GetReferenceId(id);
    }

    public Task<bool> GetSubmitApplication(int id)
    {
        return repositoryForApplication.GetSubmitApplication(id);
    }

    public async Task<string> GetUltimateController(int id)
    {
        var value = await repositoryForApplication.GetUltimateController(id);

        return responseConverter.ConvertToYesOrNo(value);
    }

    public Task<string> GetUltimateControllerName(int applicationId, int ultimateControllerId)
    {
        return repositoryForUltimateController.Get(applicationId, ultimateControllerId);
    }

    public async Task<bool> IsCrsOrHardToReplace(int id)
    {
        var applicationCodeId = await repositoryForApplication.GetApplicationCodeId(id);

        var isCrsOrHardToReplace = await repositoryForApplicationCode.IsCrsOrHardToReplace(applicationCodeId);
        return isCrsOrHardToReplace;
    }

    public async Task<Domain.Models.Database.ApplicationPage> OrchestrateNextPagePostDirectorCheck(
        int id,
        string directorCheck,
        bool atReviewStage)
    {
        var nextPage = Domain.Models.Database.ApplicationPage.Directors;

        if (atReviewStage)
        {
            nextPage = Domain.Models.Database.ApplicationPage.Review;
        }

        if (directorCheck == ApplicationFormConstants.No)
        {
            if (!atReviewStage)
            {
                nextPage = Domain.Models.Database.ApplicationPage.CorporateDirectorCheck;
            }

            await directorOrchestration.DeleteDirectors(id, Domain.Models.Database.DirectorType.Board);
        }
        else if (directorCheck == ApplicationFormConstants.Yes)
        {
            var numberOfDirectors = await directorOrchestration.CountDirectorsOfGroupType(id, Domain.Models.Database.DirectorType.Board);

            if (numberOfDirectors == 0)
            {
                nextPage = Domain.Models.Database.ApplicationPage.Directors;
            }
        }

        await SetCurrentPage(id, nextPage);

        return nextPage;
    }

    public async Task<Domain.Models.Database.ApplicationPage> DetermineNextPageAfterDirectorsSection(int id)
    {
        var applicationCodeId = await repositoryForApplication.GetApplicationCodeId(id);

        var isCrsOrHardToReplace = await repositoryForApplicationCode.IsCrsOrHardToReplace(applicationCodeId);

        if (isCrsOrHardToReplace)
        {
            return Domain.Models.Database.ApplicationPage.UltimateController;
        }

        return Domain.Models.Database.ApplicationPage.Review;
    }

    public Domain.Models.Database.ApplicationPage DetermineNextPageAfterParentCompaniesCheck(string response, bool reviewPage)
    {
        if(response == ApplicationFormConstants.Yes)
        {
            return Domain.Models.Database.ApplicationPage.ParentCompanies;
        }

        if (reviewPage)
        {
            return Domain.Models.Database.ApplicationPage.Review;
        }

        return  Domain.Models.Database.ApplicationPage.DirectorsSatisfyG3FitAndProperRequirements;
    }

    public async Task SeedApplication(
        int applicationId,
        int? preApplicationId,
        int applicationCodeId)
    {
        if(preApplicationId != null)
        {
            var preApplication = await repositoryForPreApplication.GetByIdAsync(preApplicationId.Value);

            if(preApplication != null)
            {
                await repositoryForApplication.SetCQCProviderName(applicationId, preApplication.CQCProviderName);
                await repositoryForApplication.SetCQCProviderID(applicationId, preApplication.CQCProviderID);
                await repositoryForApplication.SetCQCProviderAddress(applicationId, preApplication.CQCProviderAddress);
                await repositoryForApplication.SetCQCProviderPhoneNumber(applicationId, preApplication.CQCProviderPhoneNumber);

                /* we don't allow the applicant to change/persist the CQC website url in the pre application */
                var cqcProviderId = await repositoryForCQCProvider.GetIdIfExistsAsync(preApplication.CQCProviderID);

                if(cqcProviderId != null)
                {
                    var provider = await repositoryForCQCProvider.GetByIdAsync(cqcProviderId.Value);

                    if(provider != null)
                    {
                        await repositoryForApplication.SetCQCProviderWebsiteURL(applicationId, provider.WebsiteURL ?? string.Empty);
                    }
                }

                await SetContactDetails(applicationId, new ContactDetailsDTO()
                {
                    Forename = preApplication.FirstName,
                    Surname = preApplication.LastName,
                    JobTitle = preApplication.JobTitle,
                    Email = preApplication.Email,
                    ElectronicCommunications = true
                });

                return;
            }
        }

        var applicationCode = await repositoryForApplicationCode.GetByIdAsync(applicationCodeId);

        if(applicationCode != null)
        {
            await SetContactDetails(applicationId, new ContactDetailsDTO()
            {
                Forename = applicationCode.NoPreApplication_FirstName ?? string.Empty,
                Surname = applicationCode.NoPreApplication_LastName ?? string.Empty,
                JobTitle = string.Empty,
                Email = applicationCode.NoPreApplication_Email ?? string.Empty,
                ElectronicCommunications = true
            });
        }

        return;
    }

    public async Task<ApplicationCodeOrchestrationResult> OrchestrateApplicationCodeAsync(
        string code,
        string userId)
    {
        var applicationCodeDetails = await repositoryForApplicationCode.GetApplicationCodeDetailsAsync(code, userId);

        if(applicationCodeDetails != null)
        {
            if(applicationCodeDetails.ApplicationId == null)
            {
                var nextPage = Domain.Models.Database.ApplicationPage.CQCProviderID;

                var applicationId = await repositoryForApplication.AddAsync(new()
                {
                    ApplicationCodeId = applicationCodeDetails.ApplicationCodeId,
                    CurrentPageId = (int)nextPage
                });

                await SeedApplication(
                    applicationId,
                    applicationCodeDetails.PreApplicationId,
                    applicationCodeDetails.ApplicationCodeId);

                if (applicationCodeDetails.PreApplicationId != null)
                {
                    nextPage  = Domain.Models.Database.ApplicationPage.CQCProviderConfirmation;
                }

                return new ApplicationCodeOrchestrationResult()
                {
                    ApplicationId = applicationId,
                    Exists = true,
                    NextPage = nextPage,
                    FirstTimeAccessingApplication = true
                };
            }

            return new ApplicationCodeOrchestrationResult()
            {
                ApplicationId = (int)applicationCodeDetails.ApplicationId,
                Exists = true,
                NextPage = applicationCodeDetails.CurrentPage ?? throw new NotImplementedException($"The current page should always be set - but alas this time its not: {applicationCodeDetails.ApplicationId}"),
                FirstTimeAccessingApplication = false
            };
        }

        return new ApplicationCodeOrchestrationResult()
        {
            Exists = false
        };
    }

    public async Task<string> SaveAndExitAsync(
        int id,
        string applicationURL)
    {
        var emailNotification = new EmailNotification()
        {
            ApplicationId = id,
            TypeId = (int)Domain.Models.Database.EmailNotificationType.MainApplicationSaveAndExit,
            ApplicationURL = applicationURL
        };

        await repositoryForEmailNotifications.AddAsync(emailNotification);

        var contactDetails = await repositoryForApplication.GetContactDetails(id);

        await storageAccountQueueWrapper.PutMessageOntoEmailNotificationQueue(emailNotification.Id);

        return contactDetails.Email;
    }

    public Task SetCompanyNumber(int id, string response)
    {
        return repositoryForApplication.SetCompanyNumber(id, response);
    }

    public async Task SetCompanyNumberCheck(int id, string response)
    {
        if(response == ApplicationFormConstants.No)
        {
            await SetCompanyNumber(id, string.Empty);
        }

        await repositoryForApplication.SetCompanyNumberCheck(id, responseConverter.Convert(response));
    }

    public async Task SetDirectorsCheck(int id, string response)
    {
        await repositoryForApplication.SetDirectorsCheck(id, responseConverter.Convert(response));
    }

    public async Task SetCorporateDirectorsCheck(int id, string response)
    {
        var value = responseConverter.Convert(response);

        if (value == false)
        {
            await directorOrchestration.DeleteGroups(id, Domain.Models.Database.DirectorType.Corporate);
        }

        await repositoryForApplication.SetCorporateDirectorsCheck(id, value);
    }

    public Task SetContactDetails(int id, ContactDetailsDTO contactDetails)
    {
        return repositoryForApplication.Set(id, contactDetails);
    }

    public Task SetCQCProviderAddress(int id, string response)
    {
        return repositoryForApplication.SetCQCProviderAddress(id, response);
    }

    public Task SetCQCProviderDetails(int id, CQCProviderDetailsWithoutIdDTO details)
    {
        return repositoryForApplication.SetCQCProviderDetails(id, details);
    }

    public Task SetCQCProviderID(int id, string response)
    {
        return repositoryForApplication.SetCQCProviderID(id, response);
    }

    public Task SetCQCProviderName(int id, string response)
    {
        return repositoryForApplication.SetCQCProviderName(id, response);
    }

    public Task SetCQCProviderPhoneNumber(int id, string response)
    {
        return repositoryForApplication.SetCQCProviderPhoneNumber(id, response);
    }

    public Task SetCQCProviderWebsiteURL(int id, string response)
    {
        return repositoryForApplication.SetCQCProviderWebsiteURL(id, response);
    }

    public Task SetCurrentPage(int id, Domain.Models.Database.ApplicationPage applicationPage)
    {
        return repositoryForApplication.SetCurrentPage(id, applicationPage);
    }

    public Task SetDirectorsSatisfyG3FitAndProperRequirements(int id, string response)
    {
        return repositoryForApplication.SetDirectorsSatisfyG3FitAndProperRequirements(id, responseConverter.Convert(response));
    }

    public Task SetDirectorsSatisfyG3FitAndProperRequirementsIfNoWhy(int id, string response)
    {
        return repositoryForApplication.SetDirectorsSatisfyG3FitAndProperRequirementsIfNoWhy(id, response);
    }

    public Task SetLastFinancialYear(int id, DateOnly? date)
    {
        return repositoryForApplication.SetLastFinancialYear(id, date);
    }

    public async Task SetNewlyIncorporatedCompany(int id, string response)
    {
        var value = responseConverter.Convert(response);

        if (value == true)
        {
            await repositoryForApplication.SetLastFinancialYear(id, null);
        }

        await repositoryForApplication.SetNewlyIncorporatedCompany(id, value);
    }

    public Task SetNextFinancialYear(int id, DateOnly? date)
    {
        return repositoryForApplication.SetNextFinancialYear(id, date);
    }

    public async Task SetOneOrMoreParentCompanies(int id, string response)
    {
        if(response == ApplicationFormConstants.No)
        {
            await directorOrchestration.DeleteGroups(id, Domain.Models.Database.DirectorType.ParentCompany);
        }

        await repositoryForApplication.SetOneOrMoreParentCompanies(id, responseConverter.Convert(response));
    }

    public async Task SetUltimateController(int id, string response)
    {
        if (response == ApplicationFormConstants.No)
        {
            /* No ultimate controller, reset name as it may have been set already */
            await repositoryForUltimateController.Delete(id);
        }

        await repositoryForApplication.SetUltimateController(id, responseConverter.Convert(response));
    }

    public async Task SetUltimateControllerName(int applicationId, int ultimateControllerId, string response)
    {
        await repositoryForUltimateController.Set(applicationId, ultimateControllerId, response);
    }

    public async Task SubmitApplication(int id)
    {
        var dateTime = dateTimeFactory.Create();

        await repositoryForApplication.SetSubmitApplication(id, dateTime);

        await repositoryForApplication.SetCurrentPage(id, Domain.Models.Database.ApplicationPage.Submitted);

        var referenceID = referenceIDFactory.CreateForApplication(dateTime, id);

        await repositoryForApplication.SetReferenceID(id, referenceID);

        var emailNotification = new EmailNotification()
        {
            ApplicationId = id,
            TypeId = (int)Domain.Models.Database.EmailNotificationType.Application,
        };

        await repositoryForEmailNotifications.AddAsync(emailNotification);

        await storageAccountQueueWrapper.PutMessageOntoEmailNotificationQueue(emailNotification.Id);
    }

    public Task<bool> UltimateControllerExists(int applicationId, int ultimateControllerId)
    {
        return repositoryForUltimateController.Exists(applicationId, ultimateControllerId);
    }

    public Task<Domain.Models.Database.ApplicationPage> GetCurrentPage(int id)
    {
        return repositoryForApplication.GetCurrentPage(id);
    }

    private static string CreateResponseForCQCProviderDetails(ApplicationDTO application)
    {
        var response = new StringBuilder();
        var hasValue = false;

        if (!string.IsNullOrWhiteSpace(application.CQCProviderName))
        {
            response.Append(application.CQCProviderName);
            hasValue = true;
        }

        if (!string.IsNullOrWhiteSpace(application.CQCProviderAddress))
        {
            if (hasValue)
            {
                response.Append(", ");
            }

            response.Append(application.CQCProviderAddress);
            hasValue = true;
        }

        if (!string.IsNullOrWhiteSpace(application.CQCProviderPhoneNumber))
        {
            if (hasValue)
            {
                response.Append(", ");
            }

            response.Append(application.CQCProviderPhoneNumber);
        }

        return response.ToString();
    }
}
