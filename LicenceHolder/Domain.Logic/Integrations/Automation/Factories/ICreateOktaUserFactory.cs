using Domain.Objects.Integrations.Automation;

namespace Domain.Logic.Integrations.Automation.Factories
{
    public interface ICreateOktaUserFactory
    {
        CreateOktaUser Create(string firstname, string lastname, string email, string organisation);
    }
}
