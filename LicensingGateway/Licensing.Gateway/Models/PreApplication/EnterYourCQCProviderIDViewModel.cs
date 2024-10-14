using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.PreApplication;

public class EnterYourCQCProviderIDViewModel : ApplicationBase
{
    [BindProperty]
    public string CQCProviderID { get; set; } = default!;
}
