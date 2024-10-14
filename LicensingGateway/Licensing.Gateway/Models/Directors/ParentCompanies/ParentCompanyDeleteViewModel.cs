using Domain.Logic.Forms.Application;
using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Directors.ParentCompanies;

public class ParentCompanyDeleteViewModel : ApplicationBase
{
    public string GroupName { get; set; } = default!;

    [BindProperty]
    public string Answer { get; set; } = default!;


    public string[] Values = [ApplicationFormConstants.Yes, ApplicationFormConstants.No];
}
