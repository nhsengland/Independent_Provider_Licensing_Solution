using Domain.Logic.Features.YourProfile.Requests;
using Domain.Objects.ViewModels.YourProfile;

namespace Domain.Logic.Features.YourProfile;

public interface IYourProfileHandler
{
    Task<ProfileViewModel> GetProfile(UserProfileRequest request, CancellationToken cancellationToken);
}
