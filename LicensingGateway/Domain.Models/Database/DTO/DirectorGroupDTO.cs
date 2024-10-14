namespace Domain.Models.Database.DTO;

public record DirectorGroupDTO
{
    public int Id { get; init; }
    public int ApplicationId { get; init; }
    public string Name { get; init; } = default!;
    public bool? OneOrMoreIndividuals { get; init; }
    public List<DirectorDTO> Directors { get; set; } = [];
}
