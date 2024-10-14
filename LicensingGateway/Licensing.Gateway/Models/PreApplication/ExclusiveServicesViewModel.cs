using Domain.Logic.Forms.PreApplication;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.PreApplication;

public class ExclusiveServicesViewModel : ApplicationBase
{

    [BindProperty]
    public string ExclusiveServices { get; set; } = default!;

    public string[] ExclusiveServicesValues = [PreApplicationFormConstants.Yes, PreApplicationFormConstants.No];

    public string ProviderName { get; set; } = default!;
}
