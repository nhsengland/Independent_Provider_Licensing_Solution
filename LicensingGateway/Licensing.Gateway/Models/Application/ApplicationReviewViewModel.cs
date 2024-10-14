using Domain.Models.Forms.Application;

namespace Licensing.Gateway.Models.Application;

public class ApplicationReviewViewModel : ApplicationBase
{
    public ApplicationReviewData ApplicationReviewData { get; set; } = default!;
}
