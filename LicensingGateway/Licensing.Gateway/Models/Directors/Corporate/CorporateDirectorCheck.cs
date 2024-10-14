using Domain.Logic.Forms.Application;
using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Directors.Corporate;

public class CorporateDirectorCheck : ApplicationBase
{

    [BindProperty]
    public string CorporateDirectorsCheck { get; set; } = default!;

    public string[] Values = [ApplicationFormConstants.Yes, ApplicationFormConstants.No];

    public string ProviderName { get; set; } = default!;
}
