using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Entites.Core;

namespace Database.Entites;

public class DirectorGroup : BaseIntEntity
{
    public int ApplicationId { get; set; }

    [Required, ForeignKey(nameof(ApplicationId))]
    public Application Application { get; set; } = default!;

    public string Name { get; set; } = default!;

    public int TypeId { get; set; }

    [Required, ForeignKey(nameof(TypeId))]
    public DirectorType Type { get; set; } = default!;

    public bool? OneOrMoreIndividuals { get; set; } = default!;

    public virtual List<Director> Directors { get; set; } = default!;
}
