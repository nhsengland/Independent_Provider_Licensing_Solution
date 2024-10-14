
namespace Domain.Models.Database.DTO;
public class DirectorDTO : DirectorNameDTO
{
    public int Id { get; init; }
    public DateOnly DateOfBirth { get; init; } = default!;
    public string GroupName { get; set; } = string.Empty;
    public int GroupId { get; init; }

    public Models.Database.DirectorType DirectorType { get; init; }
}
