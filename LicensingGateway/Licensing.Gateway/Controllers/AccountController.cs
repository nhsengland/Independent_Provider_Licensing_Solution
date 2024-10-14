using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;
using Licensing.Gateway.Models.Account;
using Domain.Logic.Forms.Application;
using Domain.Logic.Forms.Helpers.Session;

namespace Licensing.Gateway.Controllers;
public class AccountController(
    ISessionOrchestration sessionOrchestration) : Controller
{
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;

    [Route("account/sign-out")]
    public new IActionResult SignOut()
    {
        return new SignOutResult(
            new[]
            {
                    OktaDefaults.MvcAuthenticationScheme,
                    CookieAuthenticationDefaults.AuthenticationScheme,
            },
            new AuthenticationProperties { RedirectUri = "/" }
        );
    }

    [Route("account/save-and-exit")]
    public IActionResult SaveAndExit()
    {
        return new SignOutResult(
            new[]
            {
                    OktaDefaults.MvcAuthenticationScheme,
                    CookieAuthenticationDefaults.AuthenticationScheme,
            },
            new AuthenticationProperties { RedirectUri = "/account/save-and-exit-confirmation" }
        );
    }

    [Route("account/save-and-exit-confirmation")]
    public IActionResult SaveAndExitConfirmation()
    {
        return View(new SaveAndExitViewModel() { EmailAddress = sessionOrchestration.Get<string>(ApplicationFormConstants.SessionEmailAddress) ?? string.Empty });
    }
}
