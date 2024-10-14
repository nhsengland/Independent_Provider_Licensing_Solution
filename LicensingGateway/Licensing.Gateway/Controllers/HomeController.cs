using Licensing.Gateway.Factories;
using Licensing.Gateway.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Licensing.Gateway.Controllers;

public class HomeController(
    IViewModelFactory viewModelFactory) : Controller
{
    private readonly IViewModelFactory viewModelFactory = viewModelFactory;

    public IActionResult Index()
    {
        return View(viewModelFactory.Create());
    }

    [Route("[controller]/check-if-you-need-a-licence")]
    public IActionResult CheckIfYouNeedALicense()
    {
        return View(viewModelFactory.Create());
    }

    [Route("[controller]/how-to-apply-for-a-nhs-provider-licence")]
    public IActionResult HowToApplyForANHSProviderLicence()
    {
        return View(viewModelFactory.Create());
    }

    [Route("[controller]/apply-for-a-nhs-provider-licence")]
    public IActionResult ApplyForANHSProviderLicense()
    {
        return View(viewModelFactory.Create());
    }

    [Route("[controller]/page-not-found")]
    public IActionResult PageNotFound()
    {
        return View();
    }

    [Route("[controller]/403")]
    public IActionResult Forbidden()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
