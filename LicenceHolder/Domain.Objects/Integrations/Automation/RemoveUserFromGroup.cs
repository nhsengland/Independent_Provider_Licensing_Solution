namespace Domain.Objects.Integrations.Automation;

public record RemoveUserFromGroup
{
    public string GroupId { get; set; } = "licensing";
    public string UserId { get; set; } = default!;
}
