using Domain.Models.ViewModels.Application;

namespace Licensing.Gateway.Models.Application;

public class ResumeApplicationViewModel : ApplicationBase
{
    public ReviewApplicationResponsesViewModel Responses { get; set; } = default!;
}
