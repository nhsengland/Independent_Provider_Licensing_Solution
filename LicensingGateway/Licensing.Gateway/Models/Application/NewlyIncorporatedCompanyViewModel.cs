using Domain.Logic.Forms.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Application;

public class NewlyIncorporatedCompanyViewModel : ApplicationBase
{

    [BindProperty]
    public string NewlyIncorporatedCompany { get; set; } = default!;
    public string ProviderName { get; set; } = default!;

    public string[] Values = [ApplicationFormConstants.Yes, ApplicationFormConstants.No];
}
