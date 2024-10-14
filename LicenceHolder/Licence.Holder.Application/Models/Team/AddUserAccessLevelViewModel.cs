using Domain.Objects;
using Microsoft.AspNetCore.Mvc;

namespace Licence.Holder.Application.Models.Team;

public class AddUserAccessLevelViewModel
{
    [BindProperty]
    public string AccessLevel { get; set; } = default!;

    public string[] Values { get; set; } = [PageConstants.Yes, PageConstants.No];

    public bool ValidationFailure { get; set; } = false;
}
