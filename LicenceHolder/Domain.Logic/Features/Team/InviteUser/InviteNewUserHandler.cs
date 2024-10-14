using Azure.Core;
using Database.Repositories.Orchestrate;
using Database.Repositories.User;
using Domain.Objects.ViewModels.Team;

namespace Domain.Logic.Features.Team.InviteUser;

public class InviteNewUserHandler(
    IRepositoryOrchestrator repositoryOrchestrator,
    IRepositoryForUser repositoryForUser) : IInviteNewUserHandler
{
    private readonly IRepositoryOrchestrator repositoryOrchestrator = repositoryOrchestrator;
    private readonly IRepositoryForUser repositoryForUser = repositoryForUser;

    public async Task<InviteUserViewModel> GetAsync(InviteUserQuery query, CancellationToken cancellationToken)
    {

        var organisationDetails = await repositoryOrchestrator.GetOrganisationGetDetails(query.OktaUserId, cancellationToken);        

        return new InviteUserViewModel()
        {
            OrganisationName = organisationDetails.Name,
            IsCrsOrHardToReplaceOrganisation = organisationDetails.HasCrsOrHardToReplaceCompanys,
            UserRole = await repositoryForUser.GetUserRole(query.OktaUserId, cancellationToken)

        };
    }
}
