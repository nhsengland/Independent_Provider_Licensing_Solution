using Domain.Objects.Integrations.Automation;

namespace Domain.Logic.Integrations.Automation;

public interface IAutomationAPIWrapper
{
    Task<CreateOktaUserResult> CreateOktaUser(CreateOktaUser createOktaUser);

    Task RemoveOktaUserFromGroup(RemoveUserFromGroup removeUserFromGroup);

    Task SendEmail(EmailBodyTemplate emailBodyTemplate);
}
