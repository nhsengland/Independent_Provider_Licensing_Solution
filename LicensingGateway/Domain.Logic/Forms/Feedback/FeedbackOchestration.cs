using Database.Repository.Feedback;
using Domain.Logic.Forms.Helpers;
using Domain.Models.Database;

namespace Domain.Logic.Forms.Feedback;

public class FeedbackOchestration(
    IRepositoryForFeedback repositoryForFeedback,
    IDateTimeFactory dateTimeFactory) : IFeedbackOchestration
{
    private readonly IRepositoryForFeedback repositoryForFeedback = repositoryForFeedback;
    private readonly IDateTimeFactory dateTimeFactory = dateTimeFactory;

    public async Task Create(FeedbackType type, FeedbackSatisfaction satisfaction, string? howToImprove)
    {
        var truncatedHowToImprove = howToImprove == null ? string.Empty : howToImprove.Trim();

        if (truncatedHowToImprove.Length > 1200)
        {
            truncatedHowToImprove = truncatedHowToImprove.Substring(0, 1200);
        }

        await repositoryForFeedback.AddAsync(new Database.Entites.Feedback
        {
            TypeId = (int)type,
            SatisfactionId = (int)satisfaction,
            HowToImprove = truncatedHowToImprove,
            DateGenerated = dateTimeFactory.Create()
        });
    }
}
