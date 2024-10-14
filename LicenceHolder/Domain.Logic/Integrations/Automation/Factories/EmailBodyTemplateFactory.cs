using Domain.Objects.Integrations.Automation;

namespace Domain.Logic.Integrations.Automation.Factories;

public class EmailBodyTemplateFactory(
    AutomationConfiguration automationConfiguration) : IEmailBodyTemplateFactory
{
    private readonly AutomationConfiguration automationConfiguration = automationConfiguration;

    public EmailBodyTemplate CreateForExistingOktaUser(
        string email,
        string name,
        string licenseHolderURL,
        string requestorName,
        string organisationName)
    {
        return new EmailBodyTemplate()
        {
            EmailAddress = email,
            TemplateId = automationConfiguration.ExistingUserEmailTemplate,
            Personalisation = new Personalisation()
            {
                name = name,
                licenseholderurl = licenseHolderURL,
                requestorname = requestorName,
                organisationname = organisationName
            }
        };
    }

    public EmailBodyTemplate CreateForNewOktaUser(
        string email,
        string name,
        string oktaActivationURL,
        string licenseHolderURL,
        string requestorName,
        string organisationName)
    {
        return new EmailBodyTemplate()
        {
            EmailAddress = email,
            TemplateId = automationConfiguration.NewUserEmailTemplate,
            Personalisation = new Personalisation()
            {
                name = name,
                oktaactivationurl = oktaActivationURL,
                licenseholderurl = licenseHolderURL,
                requestorname = requestorName,
                organisationname = organisationName
            }
        };
    }
}
