using Domain.Logic.Integration.CQC.API.Models;

namespace Functions.Factories;
public class InputModelFactory : IInputModelFactory
{
    public ProvidersInputModel Create(int pageNumber, string instanceId)
    {
        return new ProvidersInputModel() { PageNumber = pageNumber, InstanceId = Guid.Parse(instanceId) };
    }

    public ProvidersInputModel Create(string instanceId)
    {
        return new ProvidersInputModel() { InstanceId = Guid.Parse(instanceId) };
    }

    public ProviderImportInputModel Create(string providerID, string instanceId)
    {
        return new ProviderImportInputModel() { ProviderID = providerID, InstanceId = Guid.Parse(instanceId) };
    }
}
