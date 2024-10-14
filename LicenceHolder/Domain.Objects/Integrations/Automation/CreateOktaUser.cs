namespace Domain.Objects.Integrations.Automation;

public record CreateOktaUser
{
    public string Application { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PrimaryPhone { get; set; } = string.Empty;
    public string Organization { get; set; } = default!;

}
