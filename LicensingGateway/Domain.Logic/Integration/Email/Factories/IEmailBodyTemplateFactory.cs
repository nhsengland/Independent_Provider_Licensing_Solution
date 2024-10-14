using Domain.Logic.Integration.Email.Models;
using Domain.Models.Database;
using Domain.Models.Database.DTO;

namespace Domain.Logic.Integration.Email.Factories;
public interface IEmailBodyTemplateFactory
{
    EmailBodyTemplate Create(
        EmailNotificationType emailType,
        EmailNotificationDTO emailNotificationDTO);
}
