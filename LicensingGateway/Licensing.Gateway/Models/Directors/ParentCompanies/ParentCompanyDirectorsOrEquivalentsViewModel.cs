using Domain.Logic.Forms.Application;
using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Directors.ParentCompanies;

public class ParentCompanyDirectorsOrEquivalentsViewModel : ApplicationBase
{
    public string ParentCompanyName { get; set; } = default!;

    [BindProperty]
    public string Answer { get; set; } = default!;

    public string[] Values { get; set; } = [ApplicationFormConstants.Yes, ApplicationFormConstants.No];
}
