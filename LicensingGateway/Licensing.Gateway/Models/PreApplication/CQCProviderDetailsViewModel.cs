using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.PreApplication;

public class CQCProviderDetailsViewModel : ApplicationBase
{
    [BindProperty]
    public string CQCProviderID { get; set; } = default!;

    [BindProperty]
    public string Name { get; set; } = default!;

    [BindProperty]
    public string Address { get; set; } = default!;

    [BindProperty]
    public string PhoneNumber { get; set; } = default!;
}
