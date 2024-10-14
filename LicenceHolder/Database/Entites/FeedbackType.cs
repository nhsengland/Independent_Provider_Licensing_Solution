using Database.Entites.Core;

namespace Database.Entites;

public class FeedbackType : BaseEntity
{
    public Domain.Objects.Database.FeedbackType Type { get; set; } = default!;
}
