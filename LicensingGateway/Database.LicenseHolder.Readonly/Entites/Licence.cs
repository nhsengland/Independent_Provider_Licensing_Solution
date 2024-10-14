using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace Database.LicenceHolder.Readonly.Entites;

[Table(nameof(Licence))]
public class Licence
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    public bool IsActive { get; set; }
    public int CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))]
    [InverseProperty(nameof(Company.Licences))]
    public virtual Company Company { get; set; } = null!;
}
