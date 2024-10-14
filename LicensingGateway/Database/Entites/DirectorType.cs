using Database.Entites.Core;

namespace Database.Entites;

public class DirectorType : BaseIntEntity
{
    public Domain.Models.Database.DirectorType Type { get; set; } = default!;
}
