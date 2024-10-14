using Microsoft.AspNetCore.Mvc;

namespace Licence.Holder.Application.Models.Licence;

public class ChangeFinancialYearEndViewModel
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = default!;

    [BindProperty]
    public int? Day { get; set; } = default!;

    [BindProperty]
    public int? Month { get; set; } = default!;

    [BindProperty]
    public int? Year { get; set; } = default!;

    public bool ValidationFailure { get; set; } = false;

    public bool? IsValidDate { get; set; } = true;

    public bool? IsDateInPast { get; set; } = false;
    public DateTime NewFinancialYearDate { get; set; } = default!;
}
