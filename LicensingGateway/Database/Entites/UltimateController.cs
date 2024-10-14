using System.ComponentModel.DataAnnotations;
using Database.Entites.Core;

namespace Database.Entites;
public class UltimateController : BaseIntEntity
{
    public int ApplicationId { get; set; }

    [Required]
    public Application Application { get; set; } = default!;

    public string Name { get; set; } = default!;
}
