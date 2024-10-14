using Domain.Objects;
using Microsoft.AspNetCore.Mvc;

namespace Licence.Holder.Application.Models.AnnualCertificateTasks;

public class ACTUpdateSubmissionStatusViewModel
{
    [BindProperty]
    public int TaskId { get; set; }

    [BindProperty]
    public int LicenseId { get; set; }

    [BindProperty]
    public string LicenceName { get; set; } = default!;

    [BindProperty]
    public string UpdateSubmissionStatus { get; set; } = default!;

    public string[] Values { get; set; } = [PageConstants.Yes, PageConstants.No];

    [BindProperty]
    public bool ValidationFailure { get; set; } = false;
}
