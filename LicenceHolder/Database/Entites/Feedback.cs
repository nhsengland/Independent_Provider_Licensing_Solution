using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Database.Entites.Core;

namespace Database.Entites;

public class Feedback : BaseEntity
{
    public DateTime DateGenerated { get; set; }

    public int SatisfactionId { get; set; }

    [Required, ForeignKey(nameof(SatisfactionId))]
    public virtual FeedbackSatisfaction Satisfaction { get; set; } = default!;

    [MaxLength(1200)]
    public string? HowToImprove { get; set; }

    public int TypeId { get; set; }

    [Required, ForeignKey(nameof(TypeId))]
    public virtual FeedbackType Type { get; set; } = default!;
}
