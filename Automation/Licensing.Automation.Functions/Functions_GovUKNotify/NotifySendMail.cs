using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Notify.Client;

namespace Licensing.Automation.Functions
{
    public record SendMailData(string EmailAddress, string OktaUserId, string TemplateId, Dictionary<string,string> Personalisation, string AttachmentContent, string AttachmentName);

    public class NotifySendMail
    {
        private readonly ILogger _logger;
        private readonly AzureFunctionSettings _azureFunctionSettings;
        private readonly OktaConnection _oktaConnection;

        public NotifySendMail(ILoggerFactory loggerFactory, AzureFunctionSettings azureFunctionSettings, OktaConnection oktaConnection)
        {
            _logger = loggerFactory.CreateLogger<NotifySendMail>();
            _azureFunctionSettings = azureFunctionSettings;
            _oktaConnection = oktaConnection;
        }

        [Function("NotifySendMail")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, [FromBody] SendMailData data)
        {
            try
            {
                var personalisation = data.Personalisation.ToDictionary(x => x.Key, x => (dynamic)x.Value);

                var emailAddress = data.EmailAddress;

                if(string.IsNullOrEmpty(emailAddress))
                {
                    // was Okta User ID provided?
                    if (!string.IsNullOrEmpty(data.OktaUserId))
                    {
                        var user = await _oktaConnection.User.GetUserAsync(data.OktaUserId);
                        emailAddress = user.Profile.Email;
                        personalisation.Add(Utility.NOTIFY_TOKEN_FIRST_NAME, user.Profile.FirstName);
                        personalisation.Add(Utility.NOTIFY_TOKEN_LAST_NAME, user.Profile.LastName);
                    }
                    else
                    {
                        throw new Exception(Utility.NOTIFY_ERRORSTRING_NO_EMAIL_OR_ID);
                    }
                }

                var client = new NotificationClient(_azureFunctionSettings.GovUKNotifyAPIKey);

                if (!string.IsNullOrEmpty(data.AttachmentContent))
                {
                    byte[] attachmentContent = Convert.FromBase64String(data.AttachmentContent);
                    personalisation.Add(Utility.NOTIFY_TOKEN_ATTACHMENT, NotificationClient.PrepareUpload(attachmentContent, data.AttachmentName));
                }

                var response = await client.SendEmailAsync(
                    emailAddress: emailAddress,
                    templateId: data.TemplateId,
                    personalisation: personalisation
                );
                return Utility.CreateResponse(req);
            }
            catch (Exception ex)
            {
                return Utility.CreateResponse(req, ex);
            }
        }
    }
}
