using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Text;
using Domain.Logic.Integration.Email.Models;
using Domain.Models;
using Domain.Logic.Integration.Email.Configuration;
using Domain.Logic.Integration.Email.Factories;

namespace Domain.Logic.Integration.Email;
public class EmailServiceWrapper(
    ILogger<EmailServiceWrapper> logger,
    IHttpClientFactory httpClientFactory,
    EmailConfiguration emailConfiguration,
    IAccessTokenFactory accessTokenFactory) : IEmailServiceWrapper
{
    private readonly ILogger<EmailServiceWrapper> logger = logger;
    private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
    private readonly EmailConfiguration emailConfiguration = emailConfiguration;
    private readonly IAccessTokenFactory accessTokenFactory = accessTokenFactory;

    public async Task<bool> Send(EmailBodyTemplate template)
    {
        var token = await accessTokenFactory.CreateAsync();

        var httpClient = httpClientFactory.CreateClient(HttpClientConstants.HttpClientNameWithRetry);

        httpClient.BaseAddress = new Uri(emailConfiguration.APIBaseAddress);

        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var body = new StringContent(
        JsonSerializer.Serialize(template),
        Encoding.UTF8,
        System.Net.Mime.MediaTypeNames.Application.Json);

        var httpResponseMessage = await httpClient.PostAsync("/api/NotifySendMail", body);

        if(httpResponseMessage.IsSuccessStatusCode == false)
        {
            logger.LogError("Faild to trigger email send, response: {response}, {content}", httpResponseMessage.ToString(), await httpResponseMessage.Content.ReadAsStringAsync());
        }

        return httpResponseMessage.IsSuccessStatusCode;
    }
}
