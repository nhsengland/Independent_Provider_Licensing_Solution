using Database.Entites;
using Database.Repositories.Core;
using Domain.Objects.Database;

namespace Database.Repositories
{
    public interface IMessageRepository : IReadWriteRepository<Entites.Message>
    {
        Task<Message> GetMessage(int organisationId, int notificationId);

        Task<List<Message>> GetMesages(int organisationId, int skip, int take, Domain.Objects.Database.MessageType type);

        Task MarkAsRead(int organisationId, int notificationId);

        Task<int> NumberOfUnReadMessages(int organisationId, Domain.Objects.Database.MessageType type);

        Task<int> NumberOfMessages(int organisationId, Domain.Objects.Database.MessageType type);

        Task SendMessage(
            int organisationId,
            string body,
            string title,
            string from,
            DateTime sendDateTime,
            Domain.Objects.Database.MessageType type);
    }
}
