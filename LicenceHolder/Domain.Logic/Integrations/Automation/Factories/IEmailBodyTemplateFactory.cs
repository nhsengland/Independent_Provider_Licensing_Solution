using Domain.Objects.Integrations.Automation;

namespace Domain.Logic.Integrations.Automation.Factories;

public interface IEmailBodyTemplateFactory
{
    EmailBodyTemplate CreateForNewOktaUser(
        string email,
        string name,
        string oktaActivationURL,
        string licenseHolderURL,
        string requestorName,
        string organisationName);

    EmailBodyTemplate CreateForExistingOktaUser(
        string email,
        string name,
        string licenseHolderURL,
        string requestorName,
        string organisationName);
}
