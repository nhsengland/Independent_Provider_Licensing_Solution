namespace Domain.Objects.Database.DTO;

public record UserDTO
{
    public int Id { get; init; }

    public string Forename { get; init; } = default!;
    
    public string Surname { get; init; } = default!;
}
