using Domain.Objects.Integrations.Automation;

namespace Domain.Logic.Integrations.Automation.Factories;

public class RemoveUserFromGroupFactory : IRemoveUserFromGroupFactory
{
    public RemoveUserFromGroup Create(string OktaUserId)
    {
        return new RemoveUserFromGroup
        {
            UserId = OktaUserId
        };
    }
}
