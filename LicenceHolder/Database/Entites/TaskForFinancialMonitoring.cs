using Database.Entites.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entites;

[Table(nameof(TaskForFinancialMonitoring))]
public partial class TaskForFinancialMonitoring : BaseEntity
{
    public int OrganisationId { get; set; }

    public int TaskStatusId { get; set; }

    public DateOnly? DateDue { get; set; }

    public DateTime? DateCompleted { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateLastModified { get; set; } = DateTime.UtcNow;

    public string? SharePointLocation { get; set; } = null!;

    [ForeignKey(nameof(OrganisationId))]
    [InverseProperty(nameof(Organisation.Tasks))]
    [Required]
    public virtual Organisation Organisation { get; set; } = null!;

    [Required]
    public virtual TaskStatus TaskStatus { get; set; } = null!;

    public string TaskName { get; set; } = null!;

    [DefaultValue(false)]
    public bool InPortalNotificationSent { get; set; }
}
