using Domain.Objects;
using Microsoft.AspNetCore.Mvc;

namespace Licence.Holder.Application.Models.Team;

public record DeleteUserCheckViewModel : DeleteUserDetailsDTO
{
    [BindProperty]
    public string DeleteUser { get; set; } = default!;

    public string[] Values { get; set; } = [PageConstants.Yes, PageConstants.No];

    public bool ValidationFailure { get; set; } = false;
}
