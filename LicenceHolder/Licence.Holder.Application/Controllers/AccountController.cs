using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;

namespace Licence.Holder.Application.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult SignIn()
        {
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return Challenge(OktaDefaults.MvcAuthenticationScheme);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult SignOut()
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

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
