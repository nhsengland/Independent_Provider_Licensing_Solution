using Domain.Objects.Database;

namespace Domain.Logic.Features.Feedback.Requests;

public record SubmitFeedbackRequest
{
    public FeedbackSatisfaction Satisfaction { get; init; }

    public string HowToImprove { get; init; } = default!;

    public int FeedbackTypeId { get; init; }
}
