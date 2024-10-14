using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entites;

[PrimaryKey(nameof(CQCProviderId), nameof(ActivityId))]
public class CQCProviderToRegulatedActivities
{
    public int CQCProviderId { get; set; }

    [Required, ForeignKey(nameof(CQCProviderId))]
    public CQCProvider CQCProvider { get; set; } = default!;

    public int ActivityId { get; set; }

    [Required, ForeignKey(nameof(ActivityId))]
    public CQCProviderRegulatedActivity Activity { get; set; } = default!;
}
