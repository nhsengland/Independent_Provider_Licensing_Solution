using Database.Entites;
using Database.Repositories.User;
using Domain.Objects.Database.DTO;
using Microsoft.Extensions.Logging;

namespace Database.Repositories.Orchestrate;

public class RepositoryOrchestrator(
    ILogger<RepositoryOrchestrator> logger,
    IOrganisationRepository organisationRepository,
    ICompanyRepository companyRepository,
    IRepositoryForUser repositoryForUser) : IRepositoryOrchestrator
{
    private readonly ILogger<RepositoryOrchestrator> logger = logger;
    private readonly IOrganisationRepository organisationRepository = organisationRepository;
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly IRepositoryForUser repositoryForUser = repositoryForUser;

    public async Task<Organisation> GetOrganisationAsync(
        string oktaId,
        CancellationToken cancellationToken)
    {
        var userId = await repositoryForUser.GetIdAsync(oktaId, cancellationToken);

        return await organisationRepository.GetByUserIdAsync(userId, cancellationToken);
    }

    public async Task<OrganisationDTO> GetOrganisationGetDetails(
        string oktaId,
        CancellationToken cancellationToken)
    {
        var organisation = await GetOrganisationAsync(oktaId, cancellationToken);

        return new OrganisationDTO()
        {
            Name = organisation.Name,
            HasCrsOrHardToReplaceCompanys = await companyRepository.OrganisationHasCrsOrHardToReplaceCompanys(organisation.Id),
        };
    }

    public async Task<string> GetOrganisationNameAsync(
        string oktaId,
        CancellationToken cancellationToken)
    {
        return await organisationRepository.GetUsersOrganisationNameAsync(await GetUserId(oktaId, cancellationToken), cancellationToken);
    }

    public async Task<List<Entites.User>> GetUsersInMyOrganisation(
        string oktaId,
        CancellationToken cancellationToken)
    {
        var userId = await GetUserId(oktaId, cancellationToken);

        return await organisationRepository.GetOrganisationUsersAsync(userId, cancellationToken);
    }

    public async Task<bool> RequestingUsersOrgansiationContainsUserWithId(
        string requestingUserOktaId,
        int userId,
        CancellationToken cancellationToken)
    {
        var requestorUserId = await GetUserId(requestingUserOktaId, cancellationToken);

        var user = await repositoryForUser.GetByIdAsync(requestorUserId, cancellationToken);

        if(user == null)
        {
            logger.LogWarning("RequestorUserId {requestorUserId}, the user doesn't exist", requestorUserId);
            
            return false;
        }

        return await repositoryForUser.UserExistsInOrganisation(userId, user.OrganisationId, cancellationToken);
    }

    public async Task<bool> UserAllowedToSetAccessLevel(
        string oktaId,
        CancellationToken cancellationToken)
    {
        var organisationDetails = await GetOrganisationGetDetails(oktaId, cancellationToken);

        var userRole = await repositoryForUser.GetUserRole(oktaId, cancellationToken);

        if (organisationDetails.HasCrsOrHardToReplaceCompanys == false || userRole == Domain.Objects.Database.UserRole.Level1)
        {
            return false;
        }

        return true;
    }

    private async Task<int> GetUserId(string oktaId, CancellationToken cancellationToken)
    {
        return await repositoryForUser.GetIdAsync(oktaId, cancellationToken);
    }
}
