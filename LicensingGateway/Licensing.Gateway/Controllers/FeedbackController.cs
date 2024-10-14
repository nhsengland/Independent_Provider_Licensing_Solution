using Domain.Logic.Forms.Feedback;
using Domain.Logic.Forms.Helpers.Session;
using Domain.Models.Database;
using Licensing.Gateway.Models.Feedback;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Controllers;
public class FeedbackController(
    IFeedbackOchestration feedbackOchestration,
    ISessionOrchestration sessionOrchestration) : Controller
{
    private readonly IFeedbackOchestration feedbackOchestration = feedbackOchestration;
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;

    [Route("feeback/index")]
    public IActionResult Index()
    {
        return View(new FeedbackViewModel()
        {
            Satisfaction = sessionOrchestration.Get<FeedbackSatisfaction>(FeedbackFormConstants.SessionFeedbackSatisfaction),
            HowToImprove = sessionOrchestration.Get<string>(FeedbackFormConstants.SessionFeedbackHowToImprove),
            ValidationFailure = GetAndResetSessionValidationFailure()
        });
    }

    [HttpPost]
    [Route("feeback/index")]
    public async Task<IActionResult> Index(FeedbackViewModel model)
    {
        SetSessionVariables(model);

        if(model.Satisfaction == 0 || string.IsNullOrWhiteSpace(model.HowToImprove) || model.HowToImprove.Length > 1200)
        {
            SetSessionValidationFailure();
            return RedirectToAction();
        }

        var feedbackType = GetFeedbackType();

        await feedbackOchestration.Create((Domain.Models.Database.FeedbackType)feedbackType, model.Satisfaction, model.HowToImprove);

        return RedirectToAction(nameof(Submitted));
    }

    [Route("feeback/submitted")]
    public IActionResult Submitted()
    {
        ResetSession();
        return View();
    }

    [Route("feeback/cancel")]
    public IActionResult Cancel()
    {
        ResetSession();
        return RedirectToAction("Index", "Home");
    }

    private int GetFeedbackType()
    {
        var feedbackType = sessionOrchestration.Get<int>(FeedbackFormConstants.SessionFeedbackTypeId);
        return feedbackType == 0 ? (int)FeedbackType.Other : feedbackType;
    }

    private void ResetSession()
    {
        sessionOrchestration.Remove(FeedbackFormConstants.SessionFeedbackTypeId);
        sessionOrchestration.Remove(FeedbackFormConstants.SessionFeedbackSatisfaction);
        sessionOrchestration.Remove(FeedbackFormConstants.SessionFeedbackHowToImprove);
    }

    private void SetSessionVariables(FeedbackViewModel model)
    {
        sessionOrchestration.Set(FeedbackFormConstants.SessionFeedbackSatisfaction, model.Satisfaction);
        sessionOrchestration.Set(FeedbackFormConstants.SessionFeedbackHowToImprove, model.HowToImprove);
    }

    private bool GetAndResetSessionValidationFailure()
    {
        var result = sessionOrchestration.Get<bool>(FeedbackFormConstants.SessionFeedbackValidationFailures);

        sessionOrchestration.Remove(FeedbackFormConstants.SessionFeedbackValidationFailures);

        return result;
    }

    private void SetSessionValidationFailure()
    {
        sessionOrchestration.Set(FeedbackFormConstants.SessionFeedbackValidationFailures, true);
    }
}
