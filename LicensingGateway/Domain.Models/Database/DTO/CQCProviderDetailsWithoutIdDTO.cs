namespace Domain.Models.Database.DTO;
public record CQCProviderDetailsWithoutIdDTO
{
    public string Name { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string WebsiteURL { get; init; } = default!;
}
