using Domain.Logic.Integration.CQC.API.Models;

namespace Functions.Factories;
public interface IInputModelFactory
{
    ProvidersInputModel Create(int pageNumber, string instanceId);

    ProvidersInputModel Create(string instanceId);

    ProviderImportInputModel Create(string providerID, string instanceId);
}
