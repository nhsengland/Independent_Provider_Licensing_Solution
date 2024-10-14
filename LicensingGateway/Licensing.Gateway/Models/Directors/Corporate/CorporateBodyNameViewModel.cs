using Licensing.Gateway.Models.Application;

namespace Licensing.Gateway.Models.Directors.Corporate;

public class CorporateBodyNameViewModel : ApplicationBase
{
    public string CorporateBodyName { get; set; } = default!;

    public string ProviderName { get; set; } = default!;
}
