namespace Licence.Holder.Application.Models.Team;

public class UserChangeAccessLevelViewModel : AddUserAccessLevelViewModel
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;
}
