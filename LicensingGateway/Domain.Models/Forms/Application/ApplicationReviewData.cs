using Domain.Models.Forms.Rules;
using Domain.Models.ViewModels.Application;

namespace Domain.Models.Forms.Application;

public record ApplicationReviewData
{
    public ReviewApplicationResponsesViewModel Responses { get; init; } = default!;

    public RuleOutcomeDTO RuleOutcomeDTO { get; init; } = default!;
}
