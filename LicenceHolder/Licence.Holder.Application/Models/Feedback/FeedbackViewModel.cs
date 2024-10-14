using Domain.Objects.Database;

namespace Licence.Holder.Application.Models.Feedback;

public record FeedbackViewModel
{
    public bool ValidationFailure { get; set; } = false;
    public FeedbackSatisfaction Satisfaction { get; set; }
    public string? HowToImprove { get; set; }
}

