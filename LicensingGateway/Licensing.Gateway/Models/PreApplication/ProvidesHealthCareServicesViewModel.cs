using Domain.Logic.Forms.PreApplication;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.PreApplication;

public class ProvidesHealthCareServicesViewModel : ApplicationBase
{

    [BindProperty]
    public string ProvidesHealthCareService { get; set; } = default!;

    public string[] ProvidesHealthCareServiceValues = [PreApplicationFormConstants.Yes, PreApplicationFormConstants.No];

    public string ProviderName { get; set; } = default!;
}
