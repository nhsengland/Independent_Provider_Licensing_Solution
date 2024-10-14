namespace Domain.Logic.Integration.Email.Factories;
public interface IAccessTokenFactory
{
    Task<string> CreateAsync();
}
