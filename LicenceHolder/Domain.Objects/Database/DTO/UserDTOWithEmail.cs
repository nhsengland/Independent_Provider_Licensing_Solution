namespace Domain.Objects.Database.DTO;

public record UserDTOWithEmail : UserDTO
{    
    public string Email { get; init; } = default!;
}
