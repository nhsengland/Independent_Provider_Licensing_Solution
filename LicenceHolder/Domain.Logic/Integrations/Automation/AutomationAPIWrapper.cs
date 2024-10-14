using Domain.Objects;
using Domain.Objects.Integrations.Automation;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;
using Domain.Logic.Integrations.Automation.Factories;

namespace Domain.Logic.Integrations.Automation;

public class AutomationAPIWrapper(
    ILogger<AutomationAPIWrapper> logger,
    IHttpClientFactory httpClientFactory,
    AutomationConfiguration configuration,
    IAccessTokenFactory accessTokenFactory) : IAutomationAPIWrapper
{
    private readonly ILogger<AutomationAPIWrapper> logger = logger;
    private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
    private readonly AutomationConfiguration configuration = configuration;
    private readonly IAccessTokenFactory accessTokenFactory = accessTokenFactory;

    public async Task<CreateOktaUserResult> CreateOktaUser(CreateOktaUser createOktaUser)
    {
        var httpClient = await SetupClient();

        var body = SerialiseBody(createOktaUser);

        var httpResponseMessage = await httpClient.PostAsync("/api/OktaEnsureUser", body);

        if (httpResponseMessage.IsSuccessStatusCode == true)
        {
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            var responseObject = JsonSerializer.Deserialize<CreateOktaUserResult>(responseContent);

            if (responseObject != null)
            {
                return responseObject;
            }

            throw new Exception($"Failed to deserialize response: {responseContent}");
        }
        else
        {
            logger.LogError("BODY {body}, Faild to create okta user, response: {response}, {content}", body.ReadAsStream(), httpResponseMessage.ToString(), await httpResponseMessage.Content.ReadAsStringAsync());

            throw new Exception("Failed to create okta user");
        }
    }

    public async Task RemoveOktaUserFromGroup(
        RemoveUserFromGroup removeUserFromGroup)
    {
        var httpClient = await SetupClient();
        
        var body = SerialiseBody(removeUserFromGroup);

        var httpResponseMessage = await httpClient.PostAsync("/api/OktaRemoveUserFromGroup", body);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return;
        }

        logger.LogError("Faild to remove user from group, response: {response}, {content}", httpResponseMessage.ToString(), await httpResponseMessage.Content.ReadAsStringAsync());

        throw new Exception($"Faild to remove user from group: {removeUserFromGroup.UserId}");
    }

    public async Task SendEmail(EmailBodyTemplate emailBodyTemplate)
    {
        var httpClient = await SetupClient();

        var body = SerialiseBody(emailBodyTemplate);

        var httpResponseMessage = await httpClient.PostAsync("/api/NotifySendMail", body);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return;
        }

        logger.LogError("Faild to send email, response: {response}, {content}", httpResponseMessage.ToString(), await httpResponseMessage.Content.ReadAsStringAsync());

        throw new Exception("Faild to send email");
    }

    private static StringContent SerialiseBody(object removeUserFromGroup)
    {
        return new StringContent(
            JsonSerializer.Serialize(removeUserFromGroup),
            Encoding.UTF8,
            System.Net.Mime.MediaTypeNames.Application.Json);
    }

    private async Task<HttpClient> SetupClient()
    {
        var token = await accessTokenFactory.CreateAsync();

        var httpClient = httpClientFactory.CreateClient(HttpClientConstants.HttpClientNameWithRetry);

        httpClient.BaseAddress = new Uri(configuration.BaseUrl);

        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return httpClient;
    }
}
