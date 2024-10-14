namespace Domain.Models.Database.DTO;
public record ContactDetailsDTO
{
    public string Forename { get; init; } = default!;
    public string Surname { get; init; } = default!;
    public string JobTitle { get; init; } = default!;
    public string Email { get; init; } = default!;
    public bool ElectronicCommunications { get; init; } = default!;
}
