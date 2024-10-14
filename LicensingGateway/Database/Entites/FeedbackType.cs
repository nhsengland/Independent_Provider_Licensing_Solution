using Database.Entites.Core;

namespace Database.Entites;

public class FeedbackType : BaseIntEntity
{
    public Domain.Models.Database.FeedbackType Type { get; set; } = default!;
}
