using Database.Repositories.User;
using Domain.Logic.Features.YourProfile.Requests;
using Domain.Objects.Exceptions;
using Domain.Objects.ViewModels.YourProfile;

namespace Domain.Logic.Features.YourProfile;

public class YourProfileHandler(
    YourProfileConfiguration configuration,
    IRepositoryForUser repositoryForUser) : IYourProfileHandler
{
    private readonly YourProfileConfiguration configuration = configuration;
    private readonly IRepositoryForUser repositoryForUser = repositoryForUser;

    public async Task<ProfileViewModel> GetProfile(
        UserProfileRequest request,
        CancellationToken cancellationToken)
    {
        var user = await repositoryForUser.GetDetailsWithEmail(request.UserOktaId, cancellationToken) ?? throw new NotFoundException($"User not found: {request.UserOktaId}");

        return new ProfileViewModel
        {
            OktaUrl = configuration.OktaUrl,
            FirstName = user.Forename,
            LastName = user.Surname,
            Email = user.Email
        };
    }
}
