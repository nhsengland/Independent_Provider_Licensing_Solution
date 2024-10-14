using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.Logic.Integration.CQC.API.Factories;
using Domain.Logic.Integration.CQC.API.Models;
using Domain.Models.Exceptions;
using Microsoft.Extensions.Logging;

namespace Domain.Logic.Integration.CQC.API;
public class CQCApiWrapper(
    ILogger<CQCApiWrapper> logger,
    IHttpClientFactory httpClientFactory,
    CQCApiConfiguration configuration,
    ICQCDateFactory cQCDateFactory) : ICQCApiWrapper
{
    private readonly ILogger logger = logger;
    private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
    private readonly CQCApiConfiguration configuration = configuration;
    private readonly ICQCDateFactory cQCDateFactory = cQCDateFactory;

    public async Task<(ProviderOutputModel? model, bool isSuccess, HttpStatusCode httpStatusCode)> GetProviderAsync(string CQCProviderID)
    {
        using var client = httpClientFactory.CreateClient();

        AddHeaders(client);

        var response = await client.GetAsync($"{configuration.BaseUrl}/providers/{CQCProviderID}");

        if (response.IsSuccessStatusCode)
        {
            var output = await response.Content.ReadFromJsonAsync<ProviderOutputModel>(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            if (output == null)
            {
                throw new CQCAPIException($"Unable to obtain provider with ID {CQCProviderID}");
            }

            return (output, response.IsSuccessStatusCode, response.StatusCode);
        }

        logger.LogInformation("Failed to obtain provider with ID {CQCProviderID}, {StatusCode}", CQCProviderID, response.StatusCode);

        return (null, response.IsSuccessStatusCode, response.StatusCode);
    }

    public async Task<(ProvidersOutputModel? model, bool isSuccess)> GetProviders(int pageNumber)
    {
        using var client = httpClientFactory.CreateClient();

        AddHeaders(client);

        var response = await client.GetAsync($"{configuration.BaseUrl}/providers?perPage={configuration.PageSize}&page={pageNumber}");

        if (response.IsSuccessStatusCode)
        {
            var output = await response.Content.ReadFromJsonAsync<ProvidersOutputModel>(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            if (output == null)
            {
                throw new CQCAPIException($"Failed to obatin page number {pageNumber}");
            }

            return (output, response.IsSuccessStatusCode);
        }

        logger.LogInformation("Failed to obtain page number {pageNumber}, {StatusCode}", pageNumber, response.StatusCode);

        return (null, response.IsSuccessStatusCode);
    }

    public async Task<(ProvidersThatHaveChangedOutputModel? model, bool isSuccess)> GetProvidersThatHaveChanged(int pageNumber)
    {
        using var client = httpClientFactory.CreateClient();

        AddHeaders(client);

        var url = $"{configuration.BaseUrl}/changes/provider?perPage={configuration.PageSize}&page={pageNumber}&startTimestamp={cQCDateFactory.Create(-configuration.AmendedProvidersNumberOfDaysInThePast)}&endTimestamp={cQCDateFactory.Create(0)}";

        logger.LogInformation(url);
        
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var output = await response.Content.ReadFromJsonAsync<ProvidersThatHaveChangedOutputModel>(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            if (output == null)
            {
                throw new CQCAPIException($"Failed to obatin page number {pageNumber}");
            }

            return (output, response.IsSuccessStatusCode);
        }

        logger.LogInformation("Failed to obtain page number {pageNumber}, {StatusCode}", pageNumber, response.StatusCode);

        return (null, response.IsSuccessStatusCode);
    }

    private void AddHeaders(HttpClient client)
    {
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", configuration.OcpApimSubscriptionKey);
    }
}
