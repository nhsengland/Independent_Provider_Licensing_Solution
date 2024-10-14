namespace Domain.Models.Database.DTO;
public record CQCProviderDetailsDTO
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string WebsiteURL { get; init; } = string.Empty;
}
