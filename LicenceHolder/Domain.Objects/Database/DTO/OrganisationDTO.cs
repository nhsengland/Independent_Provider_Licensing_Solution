namespace Domain.Objects.Database.DTO;

public record OrganisationDTO
{
    public string Name { get; init; } = default!;

    public bool HasCrsOrHardToReplaceCompanys { get; init; }
}
