namespace Domain.Objects.Integrations.StorageAccounts.Queues;

public record RefreshOktaUserPermissions
{
    public string ObjectType { get; init; } = "User";
    public int ObjectId { get; init; }
}
