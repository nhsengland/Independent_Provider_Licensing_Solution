using Database.Entites.Core;

namespace Database.Entites;

public class FeedbackSatisfaction : BaseIntEntity
{
    public Domain.Models.Database.FeedbackSatisfaction Name { get; set; } = default!;
}
