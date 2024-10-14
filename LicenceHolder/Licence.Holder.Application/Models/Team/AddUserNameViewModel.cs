using Microsoft.AspNetCore.Mvc;

namespace Licence.Holder.Application.Models.Team;

public class AddUserNameViewModel
{
    [BindProperty]
    public string FirstName { get; set; } = default!;

    [BindProperty]
    public string LastName { get; set; } = default!;

    public bool ValidationFailure { get; set; } = false;
}
