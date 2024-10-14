namespace Domain.Logic.Features.Team.InviteUser;

public record AddUserHandlerConfiguration
{
    public string LicenseHolderApplicationURL { get; init; } = default!;
}
