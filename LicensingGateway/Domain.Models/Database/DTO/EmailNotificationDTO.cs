namespace Domain.Models.Database.DTO;
public record EmailNotificationDTO
{
    public int Id { get; init; }
    public int? PreApplicationId { get; init; }
    public int? ApplicationId { get; init; }
    public string? ApplicationURL { get; init; }
    public string EmailAddress { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string ApplicationReferenceId { get; set; } = default!;
    public EmailNotificationType Type { get; init; }
}
