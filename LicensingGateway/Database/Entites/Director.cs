using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Entites.Core;

namespace Database.Entites;
public class Director : BaseIntEntity
{
    public int ApplicationId { get; set; }

    [Required]
    public Application Application { get; set; } = default!;

    public string? Forename { get; set; } = default!;

    public string? Surname { get; set; } = default!;

    public DateOnly? DateOfBirth { get; set; }

    public int GroupId { get; set; }

    [ForeignKey(nameof(GroupId))]
    [InverseProperty(nameof(DirectorGroup.Directors))]
    public DirectorGroup Group { get; set; } = default!;
}
