namespace Domain.Logic.Integrations.Automation;

public record AutomationConfiguration
{
    public string BaseUrl { get; set; } = default!;
    public string NewUserEmailTemplate { get; set; } = default!;
    public string ExistingUserEmailTemplate { get; set; } = default!;
    public string APIAudienceHeader { get; set; } = default!;
}
