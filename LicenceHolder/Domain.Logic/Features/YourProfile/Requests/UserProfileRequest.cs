namespace Domain.Logic.Features.YourProfile.Requests;

public record UserProfileRequest
{
    public string UserOktaId { get; init; } = default!;
}
