using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.UltimateController;

public class UltimateControllerNameViewModel : ApplicationBase
{
    [BindProperty]
    public string Name { get; set; } = default!;
}
