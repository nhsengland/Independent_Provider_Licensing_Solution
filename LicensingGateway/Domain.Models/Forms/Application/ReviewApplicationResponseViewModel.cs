using Domain.Models.Database;

namespace Domain.Models.ViewModels.Application;

public record ReviewApplicationResponseViewModel
{
    public string Question { get; init; } = default!;
    public string Response { get; init; } = default!;
    public bool IsDate { get; init; } = false;
    public DateOnly FinancialYear { get; init; } = default;
    public ApplicationPage Page { get; init; } = default!;
    public bool IsComplete { get; init; } = default!;
    public string Controller { get; set; } = default!;
}
