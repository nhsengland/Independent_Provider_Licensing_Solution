using Domain.Logic.Forms.Application;
using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.UltimateController;

public class UltimateControllerViewModel : ApplicationBase
{
    [BindProperty]
    public string UltimateController { get; set; } = default!;

    public string[] Values = [ApplicationFormConstants.Yes, ApplicationFormConstants.No];
}
