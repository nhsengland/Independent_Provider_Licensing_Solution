using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Entites.Core;

namespace Database.Entites;

[Table(nameof(Licence))]
public partial class Licence : BaseEntity
{
    public int CompanyId { get; set; }

    [StringLength(50)]
    public string LicenceNumber { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime DateIssued { get; set; }

    public DateTime? DateCancelled { get; set; }

    [ForeignKey(nameof(CompanyId))]
    [InverseProperty(nameof(Company.Licences))]
    public virtual Company Company { get; set; } = null!;

    public string? PublishedLicenceUrl { get; set; }

    public DateTime? PublishedLicenceDate { get; set; }
}
