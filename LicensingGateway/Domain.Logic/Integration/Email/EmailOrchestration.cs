using Database.Repository.Email;
using Domain.Logic.Integration.Email.Factories;
using Domain.Models.Exceptions;
using Microsoft.Extensions.Logging;

namespace Domain.Logic.Integration.Email;
public class EmailOrchestration(
    ILogger<EmailOrchestration> logger,
    IEmailServiceWrapper emailServiceWrapper,
    IRepositoryForEmailNotifications repositoryForEmailNotifications,
    IEmailBodyTemplateFactory emailBodyTemplateFactory) : IEmailOrchestration
{
    private readonly ILogger<EmailOrchestration> logger = logger;
    private readonly IEmailServiceWrapper emailServiceWrapper = emailServiceWrapper;
    private readonly IRepositoryForEmailNotifications repositoryForEmailNotifications = repositoryForEmailNotifications;
    private readonly IEmailBodyTemplateFactory emailBodyTemplateFactory = emailBodyTemplateFactory;

    public async Task Orchestrate(int emailNotificationId)
    {
        logger.LogInformation("Start sending email notification: {emailNotificationId}", emailNotificationId);

        var emailNotification = await repositoryForEmailNotifications.GetEmailNotification(emailNotificationId);

        logger.LogInformation("Start processing email notification with ID {EmailNotificationID}", emailNotification.Id);

        var body = emailBodyTemplateFactory.Create(
            emailNotification.Type,
            emailNotification);

        var result = await emailServiceWrapper.Send(body);

        if (result)
        {
            await repositoryForEmailNotifications.MarkAsSent(emailNotification.Id);
            logger.LogInformation("Email notification with ID {EmailNotificationID} has been sent", emailNotification.Id);
        }
        else
        {
            logger.LogError("Failed to send email notification with ID {EmailNotificationID}", emailNotification.Id);
            throw new EmailNotificationException($"Failed to send email notification with ID {emailNotification.Id}");
        }
    }
}
