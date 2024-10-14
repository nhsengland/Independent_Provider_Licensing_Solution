using System.Net;
using Domain.Logic.Integration.CQC.API.Models;

namespace Domain.Logic.Integration.CQC.API;
public interface ICQCApiWrapper
{
    Task<(ProvidersOutputModel? model, bool isSuccess)> GetProviders(int pageNumber);

    Task<(ProvidersThatHaveChangedOutputModel? model, bool isSuccess)> GetProvidersThatHaveChanged(int pageNumber);

    Task<(ProviderOutputModel? model, bool isSuccess, HttpStatusCode httpStatusCode)> GetProviderAsync(string CQCProviderID);
}
