using Domain.Logic.Forms.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Application;

public class CompanyNumberCheckViewModel : ApplicationBase
{

    [BindProperty]
    public string CompanyNumberCheck { get; set; } = default!;

    public string[] Values = [ApplicationFormConstants.Yes, ApplicationFormConstants.No];
}
