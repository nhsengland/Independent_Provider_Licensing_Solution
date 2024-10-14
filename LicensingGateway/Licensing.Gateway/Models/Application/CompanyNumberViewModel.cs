namespace Licensing.Gateway.Models.Application;

public class CompanyNumberViewModel : ApplicationBase
{
    public string CompanyNumber { get; set; } = default!;

    public string Action { get; set; } = default!;

    public string Controller { get; set; } = default!;
}
