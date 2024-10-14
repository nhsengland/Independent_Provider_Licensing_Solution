using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.LicenceHolder.Readonly.Entites;

[Table("Company")]
public class Company
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    [Required, Length(5, 20)]
    public string CQCProviderID { get; set; } = default!;

    [InverseProperty(nameof(Licence.Company))]
    public virtual ICollection<Licence> Licences { get; set; } = [];
}
