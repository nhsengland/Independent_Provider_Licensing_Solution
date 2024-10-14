using Microsoft.AspNetCore.Mvc;

namespace Licence.Holder.Application.Models.Team;

public class AddUserEmailViewModel
{
    [BindProperty]
    public string Email { get; set; } = default!;

    [BindProperty]
    public string ComparisonEmail { get; set; } = default!;

    public bool ValidationFailure { get; set; } = false;

    public bool EmailDoesNotMatch { get; set; } = false;

    public bool EmailInUse { get; set; } = false;

    public bool EmailInBlackList { get; set; } = false;
}
