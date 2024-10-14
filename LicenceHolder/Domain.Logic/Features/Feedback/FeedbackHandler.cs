using Database.Repositories.Feedback;
using Domain.Logic.Factories;
using Domain.Logic.Features.Feedback.Requests;

namespace Domain.Logic.Features.Feedback;

public class FeedbackHandler(
    IRepositoryForFeedback repositoryForFeedback,
    IDateTimeFactory dateTimeFactory) : IFeedbackHandler
{
    private readonly IRepositoryForFeedback repositoryForFeedback = repositoryForFeedback;
    private readonly IDateTimeFactory dateTimeFactory = dateTimeFactory;

    public async Task HandleAsync(SubmitFeedbackRequest request)
    {
        var truncatedHowToImprove = request.HowToImprove == null ? string.Empty : request.HowToImprove.Trim();

        if (truncatedHowToImprove.Length > 1200)
        {
            truncatedHowToImprove = truncatedHowToImprove.Substring(0, 1200);
        }

        await repositoryForFeedback.AddAsync(new Database.Entites.Feedback
        {
            SatisfactionId = (int)request.Satisfaction,
            HowToImprove = truncatedHowToImprove,
            TypeId = request.FeedbackTypeId,
            DateGenerated = dateTimeFactory.Create()
        });
    }
}
