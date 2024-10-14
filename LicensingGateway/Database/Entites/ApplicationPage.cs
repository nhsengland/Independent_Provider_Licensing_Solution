using System.ComponentModel.DataAnnotations.Schema;
using Database.Entites.Core;

namespace Database.Entites;
public class ApplicationPage : BaseIntEntity
{
    [Column(TypeName = "nvarchar(50)")]
    public Domain.Models.Database.ApplicationPage PageName { get; set; } = default!;
}
