namespace Licence.Holder.Application.Models.Team;

public record DeleteUserDetailsDTO
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public bool IsVerified { get; set; }

    public DateTime? DateLastEmailNotificationWasCreated { get; set; }
}
