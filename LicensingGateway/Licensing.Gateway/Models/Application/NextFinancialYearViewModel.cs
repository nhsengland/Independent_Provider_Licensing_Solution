using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Application;

public class NextFinancialYearViewModel : ApplicationBase
{
    [BindProperty]
    public int? Day { get; set; } = default!;
    [BindProperty]
    public int? Month { get; set; } = default!;
    [BindProperty]
    public int? Year { get; set; } = default!;

    public bool? IsValidDate { get; set; } = default!;

    public string ProviderName { get; set; } = default!;
}
