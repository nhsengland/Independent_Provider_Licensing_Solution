using Domain.Objects;
using Microsoft.AspNetCore.Mvc;

namespace Licence.Holder.Application.Models.AnnualCertificateTasks;

public class FMTUpdateSubmissionStatusViewModel
{
    [BindProperty]
    public int TaskId { get; set; }

    [BindProperty]
    public int OrganisationId { get; set; }

    [BindProperty]
    public string OrganisationName { get; set; } = default!;

    [BindProperty]
    public string UpdateSubmissionStatus { get; set; } = default!;

    public string[] Values { get; set; } = [PageConstants.Yes, PageConstants.No];

    [BindProperty]
    public bool ValidationFailure { get; set; } = false;
}
