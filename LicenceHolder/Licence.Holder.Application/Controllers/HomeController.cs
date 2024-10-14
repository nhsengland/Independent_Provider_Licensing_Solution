using Domain.Logic.Features.Dashboard;
using Domain.Logic.Integrations.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Licence.Holder.Application.Controllers;

[Authorize]
public class HomeController(
    IDashboardViewModelHandler handler,
    ISessionOrchestration sessionOrchestration) : BaseController(sessionOrchestration)
{
    private readonly IDashboardViewModelHandler handler = handler;

    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var model = await handler.Get(new GetDashboardViewModelQuery()
        {
            UserId = GetOktaUserId()
        }, cancellationToken);

        return model == null ? throw new InvalidOperationException($"Failed to get dashboard: {GetOktaUserId()}") : (IActionResult)View(model);
    }

    [Route("[controller]/page-not-found")]
    public IActionResult PageNotFound()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
