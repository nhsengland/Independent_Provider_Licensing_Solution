namespace Domain.Objects.Integrations.Automation;

public record CreateOktaUserResult
{
    public bool ExistingUser { get; set; }
    public string ActivationUrl { get; set; } = default!;
    public CreateOktaUserResult_User User { get; set; } = default!;

    public record CreateOktaUserResult_User
    {
        public string Id { get; set; } = default!;
        public string Status { get; set; } = default!;
    }
}
