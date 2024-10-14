using Domain.Models.Database;

namespace Domain.Logic.Forms.Feedback;
public interface IFeedbackOchestration
{
    public Task Create(FeedbackType type, FeedbackSatisfaction satisfaction, string? howToImprove);
}
