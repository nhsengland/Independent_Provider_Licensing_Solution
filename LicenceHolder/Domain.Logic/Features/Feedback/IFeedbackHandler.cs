using Domain.Logic.Features.Feedback.Requests;

namespace Domain.Logic.Features.Feedback;

public interface IFeedbackHandler
{
    Task HandleAsync(SubmitFeedbackRequest request);
}
