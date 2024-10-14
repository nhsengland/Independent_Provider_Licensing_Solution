using Domain.Objects.Integrations.Automation;

namespace Domain.Logic.Integrations.Automation.Factories;

public interface IRemoveUserFromGroupFactory
{
    RemoveUserFromGroup Create(string OktaUserId);
}
