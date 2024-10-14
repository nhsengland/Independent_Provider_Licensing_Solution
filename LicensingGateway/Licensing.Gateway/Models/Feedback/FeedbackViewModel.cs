using Domain.Models.Database;

namespace Licensing.Gateway.Models.Feedback;

public class FeedbackViewModel
{
    public bool ValidationFailure { get; set; } = false;
    public FeedbackSatisfaction Satisfaction { get; set; }
    public string? HowToImprove { get; set; }
}
