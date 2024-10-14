using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Entites.Core;

namespace Database.Entites;

[Table(nameof(Organisation))]
public partial class Organisation : BaseEntity
{
    [StringLength(250)]
    public string Name { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime DateLastUpdated { get; set; }

    [InverseProperty(nameof(Company.Organisation))]
    public virtual ICollection<Company> Companies { get; set; } = [];

    [InverseProperty(nameof(User.Organisation))]
    public virtual ICollection<User> Users { get; set; } = [];

    [InverseProperty(nameof(TaskForFinancialMonitoring.Organisation))]
    public virtual ICollection<TaskForFinancialMonitoring> Tasks { get; set; } = [];

    public string? SharePointLocation { get; set; } = null!;

    public Guid? NHSEUserEntraId1 { get; set; }

    public Guid? NHSEUserEntraId2 { get; set; }

    public Guid? NHSEUserEntraId3 { get; set; }

    public Guid? NHSEUserEntraId4 { get; set; }

    public Guid? NHSEUserEntraId5 { get; set; }

    public Guid? NHSEUserEntraId6 { get; set; }

    [DefaultValue(false)]
    public bool IsListed { get; set; }
}
