using Domain.Logic.Forms.Application;
using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;
namespace Licensing.Gateway.Models.Directors.ParentCompanies;

public class ParentCompaniesCheckViewModel : ApplicationBase
{

    [BindProperty]
    public string Check { get; set; } = default!;

    public string[] Values = [ApplicationFormConstants.Yes, ApplicationFormConstants.No];

    public string ProviderName { get; set; } = default!;
}
