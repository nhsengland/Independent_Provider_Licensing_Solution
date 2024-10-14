namespace Domain.Logic.Integrations.Automation.Factories;

public interface IAccessTokenFactory
{
    Task<string> CreateAsync();
}
