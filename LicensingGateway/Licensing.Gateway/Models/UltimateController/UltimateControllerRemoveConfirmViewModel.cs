using Domain.Logic.Forms.Application;
using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.UltimateController;

public class UltimateControllerRemoveConfirmViewModel : ApplicationBase
{
    [BindProperty]
    public string Confirmation { get; set; } = default!;

    public string UltimateControllerName { get; set; } = default!;

    public string[] Values = [ApplicationFormConstants.Yes, ApplicationFormConstants.No];
}
