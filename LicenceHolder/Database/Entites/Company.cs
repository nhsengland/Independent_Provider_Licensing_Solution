using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Entites.Core;

namespace Database.Entites;

[Table("Company")]
public partial class Company : BaseEntity
{
    public int OrganisationId { get; set; }

    [Required, Length(5, 20)]
    public string CQCProviderID { get; set; } = default!;

    [StringLength(250)]
    public string Name { get; set; } = null!;

    [Required]
    public DateOnly FinancialYearEnd { get; set; }

    [Required]
    public string Address { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime DateLastUpdated { get; set; }

    [InverseProperty(nameof(Licence.Company))]
    public virtual ICollection<Licence> Licences { get; set; } = [];

    [Required]
    public virtual Organisation Organisation { get; set; } = null!;

    [InverseProperty(nameof(TaskForAnnualCertificate.Company))]
    public virtual ICollection<TaskForAnnualCertificate> Tasks { get; set; } = [];

    [Required, DefaultValue(false)]
    public bool IsCrs { get; set; }

    [Required, DefaultValue(false)]
    public bool IsHardToReplace { get; set; }

    [Required, DefaultValue(false)]
    public bool IsActive { get; set; }

    public string? SharePointLocation { get; set; } = null!;
}
