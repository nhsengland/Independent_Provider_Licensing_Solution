using Domain.Logic.Features.YourProfile;
using Domain.Logic.Features.YourProfile.Requests;
using Domain.Logic.Integrations.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Licence.Holder.Application.Controllers;

[Authorize]
public class YourProfileController(
    IYourProfileHandler handler,
    ISessionOrchestration sessionOrchestration) : BaseController(sessionOrchestration)
{
    private readonly IYourProfileHandler handler = handler;
    
    [Route("your-profile")]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var model = await handler.GetProfile(new UserProfileRequest()
        {
            UserOktaId = GetOktaUserId()
        }, cancellationToken);

        return model == null ? throw new InvalidOperationException($"Failed to get dashboard: {GetOktaUserId()}") : (IActionResult)View(model);
    }

    [Route("your-profile/feedback")]
    public IActionResult Feedback()
    {
        Set_Session_FeedbackType(Domain.Objects.Database.FeedbackType.YourProfile);

        return RedirectToAction("Index", "Feedback");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
