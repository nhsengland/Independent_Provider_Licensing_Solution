using Database.Entites;
using Database.Repository.Core.ReadWrite;
using Domain.Models.Database.DTO;

namespace Database.Repository.Email;
public interface IRepositoryForEmailNotifications : IReadWriteIntPkRepository<EmailNotification>
{
    Task<List<EmailNotificationDTO>> GetUnSentEmailNotifications();

    Task<EmailNotificationDTO> GetEmailNotification(int id);

    Task MarkAsSent(int id);
}
