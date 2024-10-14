namespace Domain.Objects.ViewModels.Team;

public record ManageUserViewModel
{
    public int Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public DateTime? DateLastEmailNotificationWasCreated { get; set; }

    public bool IsVerified { get; set; }

    public string AccessLevel { get; set; } = default!;

    public bool ShowAccessLevel { get; set; } = false;

    public bool EmailIsVerified { get; set; }

}
