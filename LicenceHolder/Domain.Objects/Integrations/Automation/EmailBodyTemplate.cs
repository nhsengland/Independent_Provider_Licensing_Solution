namespace Domain.Objects.Integrations.Automation;

public record EmailBodyTemplate
{
    public string EmailAddress { get; init; } = default!;
    public string TemplateId { get; init; } = default!;
    public Personalisation Personalisation { get; init; } = default!;
}

public record Personalisation
{
    public string name { get; init; } = default!;
    public string oktaactivationurl { get; init; } = default!;
    public string licenseholderurl { get; init; } = default!;
    public string requestorname { get; init; } = default!;
    public string organisationname { get; init; } = default!;
}
