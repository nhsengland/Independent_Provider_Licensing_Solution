using Domain.Logic.Integration.Email.Configuration;
using Domain.Logic.Integration.Email.Models;
using Domain.Models.Database;
using Domain.Models.Database.DTO;

namespace Domain.Logic.Integration.Email.Factories;
public class EmailBodyTemplateFactory(
    EmailConfiguration emailConfiguration) : IEmailBodyTemplateFactory
{
    private readonly EmailConfiguration emailConfiguration = emailConfiguration;

    public EmailBodyTemplate Create(
        EmailNotificationType emailType,
        EmailNotificationDTO emailNotificationDTO)
    {
        return new EmailBodyTemplate
        {
            TemplateId = emailType switch
            {
                EmailNotificationType.PreApplication => emailConfiguration.PreApplicationTemplateID,
                EmailNotificationType.Application => emailConfiguration.ApplicationTemplateID,
                EmailNotificationType.MainApplicationSaveAndExit => emailConfiguration.MainApplicationSaveAndExitTemplateID,
                _ => throw new ArgumentOutOfRangeException(nameof(emailType), emailType, null)
            },
            EmailAddress = emailNotificationDTO.EmailAddress,
            Personalisation = emailType switch
            {
                EmailNotificationType.PreApplication or EmailNotificationType.Application => new Personalisation()
                {
                    firstname = emailNotificationDTO.Name,
                    applicationReferenceID = emailNotificationDTO.ApplicationReferenceId
                },
                EmailNotificationType.MainApplicationSaveAndExit => new PersonalisationSaveAndExit()
                {
                    applicationURL = emailNotificationDTO.ApplicationURL ?? throw new ArgumentNullException(nameof(emailNotificationDTO.ApplicationURL)),
                },
                _ => throw new ArgumentOutOfRangeException(nameof(emailType), emailType, null)
            }
        };
    }
}
