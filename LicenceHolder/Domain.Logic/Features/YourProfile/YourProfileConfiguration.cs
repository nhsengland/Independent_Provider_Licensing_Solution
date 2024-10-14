namespace Domain.Logic.Features.YourProfile;

public record YourProfileConfiguration
{
    public string OktaUrl { get; init; } = default!;
}
