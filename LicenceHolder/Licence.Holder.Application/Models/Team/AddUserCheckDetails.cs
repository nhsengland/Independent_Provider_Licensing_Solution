namespace Licence.Holder.Application.Models.Team;

public class AddUserCheckDetails
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string AccessLevel { get; set; } = default!;

    public bool ShowAccessLevel { get; set; } = false;
}
