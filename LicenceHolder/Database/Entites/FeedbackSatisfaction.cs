using Database.Entites.Core;

namespace Database.Entites;

public class FeedbackSatisfaction : BaseEntity
{
    public Domain.Objects.Database.FeedbackSatisfaction Name { get; set; } = default!;
}