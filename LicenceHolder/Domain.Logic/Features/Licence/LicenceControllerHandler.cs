using Database.Entites;
using Database.Repositories.ChangeRequests;
using Database.Repositories.License;
using Database.Repositories.User;
using Domain.Logic.Features.Licence.Queries;
using Domain.Objects.Exceptions;
using Domain.Objects.ViewModels.Licence;
using Database.Repositories;
using Domain.Logic.Features.Messages;
using Domain.Logic.Factories;

namespace Domain.Logic.Features.Licence;

public class LicenceControllerHandler(
    IRepositoryForUser repositoryForUser,
    IRepositoryForLicense repositoryForLicense,
    IRepositoryForChangeRequests repositoryForChangeRequests,
    IDateEvaluation dateEvaluation,
    IMessagesHandler messageHandler,
    IOrganisationRepository organisationRepository,
    IDateTimeFactory dateTimeFactory,
    IAddressConcatenation addressConcatenation) : ILicenceControllerHandler
{
    private readonly IRepositoryForUser repositoryForUser = repositoryForUser;
    private readonly IRepositoryForLicense repositoryForLicense = repositoryForLicense;
    private readonly IRepositoryForChangeRequests repositoryForChangeRequests = repositoryForChangeRequests;
    private readonly IDateEvaluation dateEvaluation = dateEvaluation;
    private readonly IMessagesHandler messageHandler = messageHandler;
    private readonly IOrganisationRepository organisationRepository = organisationRepository;
    private readonly IDateTimeFactory dateTimeFactory = dateTimeFactory;
    private readonly IAddressConcatenation addressConcatenation = addressConcatenation;

    public async Task Create(CompanyAddressChangeRequest request, CancellationToken cancellationToken)
    {
        var licence = await EnsureRequestForLicenceIsValid(request.UserOktaId, request.LicenseId, cancellationToken);

        var user = await repositoryForUser.GetDetails(request.UserOktaId, cancellationToken);

        var now = dateTimeFactory.Create();

        var address = addressConcatenation.Concat(
                request.Address_Line_1,
                request.Address_Line_2,
                request.Address_TownOrCity,
                request.Address_County,
                request.Address_Postcode);

        await repositoryForChangeRequests.AddAsync(new ChangeRequest()
        {
            CompanyId = request.CompanyId,
            TypeId = (int)Objects.Database.ChangeRequestType.Address,
            StatusId = (int)Objects.Database.ChangeRequestStatus.Pending,
            Address = address,
            DateLastUpdated = now,
            RaisedById = user.Id,
        }, cancellationToken);

        await messageHandler.SendAsync(new Messages.Requests.ChangeRequestForCompanyAddress
        {
            OrganisationId = licence.Company.OrganisationId,
            CompanyName = licence.Company.Name,
            LicenseNumber = licence.LicenceNumber,
            RequestorName = $"{user.Forename} {user.Surname}",
            RequestedOn = now,
            PreviousAddress = licence.Company.Address,
            NewAddress = address
        }, cancellationToken);
    }

    public async Task Create(CompanyFYEChangeRequest request, CancellationToken cancellationToken)
    {
        var licence = await EnsureRequestForLicenceIsValid(request.UserOktaId, request.LicenseId, cancellationToken);

        var user = await repositoryForUser.GetDetails(request.UserOktaId, cancellationToken);

        var now = dateTimeFactory.Create();

        await repositoryForChangeRequests.AddAsync(new ChangeRequest()
        {
            CompanyId = request.CompanyId,
            TypeId = (int)Objects.Database.ChangeRequestType.FinancialYearEnd,
            StatusId = (int)Objects.Database.ChangeRequestStatus.Pending,
            FinancialYearEnd = request.FinancialYearEnd,
            DateLastUpdated = now,
            RaisedById = user.Id,
        }, cancellationToken);

        await messageHandler.SendAsync(new Messages.Requests.ChangeRequestForFYE
        {
            OrganisationId = licence.Company.OrganisationId,
            CompanyName = licence.Company.Name,
            LicenseNumber = licence.LicenceNumber,
            RequestorName = $"{user.Forename} {user.Surname}",
            RequestedOn = now,
            PreviousFinancialYearEnd = licence.Company.FinancialYearEnd,
            NewFinancialYearEnd = request.FinancialYearEnd
        }, cancellationToken);
    }

    public async Task Create(CompanyNameChangeRequest request, CancellationToken cancellationToken)
    {
        var licence = await EnsureRequestForLicenceIsValid(request.UserOktaId, request.LicenseId, cancellationToken);

        var user = await repositoryForUser.GetDetails(request.UserOktaId, cancellationToken);

        var now = dateTimeFactory.Create();

        await repositoryForChangeRequests.AddAsync(new ChangeRequest()
        {
            CompanyId = request.CompanyId,
            TypeId = (int)Objects.Database.ChangeRequestType.Name,
            StatusId = (int)Objects.Database.ChangeRequestStatus.Pending,
            Name = request.Name,
            DateLastUpdated = now,
            RaisedById = user.Id,
        }, cancellationToken);

        await messageHandler.SendAsync(new Messages.Requests.ChangeRequestForCompanyName
        {
            OrganisationId = licence.Company.OrganisationId,
            PreviousCompanyName = licence.Company.Name,
            NewCompanyName = request.Name,
            LicenseNumber = licence.LicenceNumber,
            RequestorName = $"{user.Forename} {user.Surname}",
            RequestedOn = now,
        }, cancellationToken);
    }

    public async Task<LicenceDetailsViewModel> GetLicenceDetailsAsync(LicenceRequest request, CancellationToken cancellationToken)
    {
        var licence = await EnsureRequestForLicenceIsValid(request.UserOktaId, request.LicenseId, cancellationToken);

        return new LicenceDetailsViewModel
        {
            CompanyId = licence.Company.Id,
            CompanyName = licence.Company.Name,
            CQCProviderID    = licence.Company.CQCProviderID,
            Address = licence.Company.Address,
            FinancialYearEnd = licence.Company.FinancialYearEnd,
            LicenseNumber = licence.LicenceNumber,
            LicenseId = licence.Id,
            PublishedLicenceUrl = licence.PublishedLicenceUrl,
        };
    }

    public DateOnly? IfValidDateConvert(string date)
    {
        return dateEvaluation.EvaluateDate(date);
    }

    public bool IsDateGreaterThanToday(DateOnly? date)
    {
        if(date == null)
        {
            return false;
        }

        if(date > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            return true;
        }

        return false;
    }

    private async Task<Database.Entites.Licence> EnsureRequestForLicenceIsValid(string userOktaId, int licenseId, CancellationToken cancellationToken)
    {
        var userId = await repositoryForUser.GetIdAsync(userOktaId, cancellationToken);

        var license = await repositoryForLicense.GetWithCompanyAsync(licenseId);

        var isUserInOrganisation = await repositoryForUser.UserExistsInOrganisation(userId, license.Company.OrganisationId, cancellationToken);

        if (!isUserInOrganisation)
        {
            throw new NotAuthorisedException($"User {userId} is not in the organisation {license.Company.OrganisationId}");
        }

        return license;
    }
}
