using Domain.Objects.Integrations.Automation;

namespace Domain.Logic.Integrations.Automation.Factories
{
    public class CreateOktaUserFactory : ICreateOktaUserFactory
    {
        public CreateOktaUser Create(string firstname, string lastname, string email, string organisation)
        {
            return new CreateOktaUser
            {
                Application = "licensing",
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                Organization = organisation
            };
        }
    }
}
