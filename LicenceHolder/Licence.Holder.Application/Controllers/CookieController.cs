using Domain.Objects;
using Licence.Holder.Application.Models.Cookie;
using Microsoft.AspNetCore.Mvc;

namespace Licence.Holder.Application.Controllers
{
    public class CookieController : Controller
    {
        [Route("cookie-policy")]
        public IActionResult Index()
        {
            var viewModel = new CookieViewModel();

            var currentValue = Request.Cookies[CookieConstants.CookieName];

            if (!string.IsNullOrWhiteSpace(currentValue))
            {
                foreach (var possibleValue in viewModel.Values)
                {
                    if (possibleValue == currentValue)
                    {
                        viewModel.SelectedValue = currentValue;
                    }
                }
            }

            return View(viewModel);
        }

        [Route("cookie-policy")]
        [HttpPost]
        public IActionResult Index(CookieViewModel model)
        {
            Response.Cookies.Append(CookieConstants.CookieName, model.SelectedValue, CreateCookieOptions());

            return RedirectToAction(nameof(Confirmation));
        }

        [Route("cookie-confirmation")]
        public IActionResult Confirmation()
        {
            return View();
        }

        [Route("accept-cookies")]
        [HttpPost]
        public IActionResult AcceptCookies()
        {
            Response.Cookies.Append(CookieConstants.CookieName, CookieConstants.CookieValue_Accept, CreateCookieOptions());

            return Ok();
        }

        [Route("decline-cookies")]
        [HttpPost]
        public IActionResult DeclineCookies()
        {
            Response.Cookies.Append(CookieConstants.CookieName, CookieConstants.CookieValue_Decline, CreateCookieOptions());

            return Ok();
        }

        private static CookieOptions CreateCookieOptions()
        {
            return new CookieOptions()
            {
                Expires = new DateTimeOffset(DateTime.UtcNow.AddYears(1))
            };
        }
    }
}
