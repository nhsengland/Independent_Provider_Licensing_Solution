namespace Domain.Objects.ViewModels.Team;

public record ManageUserChangeAccessLevelViewModel
{
    public int Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string AccessLevel { get; set; } = default!;
}
