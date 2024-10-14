using Licensing.Gateway.Models.Application;

namespace Licensing.Gateway.Models.Directors.ParentCompanies;

public class ParentCompanyNameViewModel : ApplicationBase
{
    public string ParentCompanyName { get; set; } = default!;

    public string ProviderName { get; set; } = default!;
}
