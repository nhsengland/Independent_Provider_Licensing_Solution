using Database.Contexts;
using Database.Entites;
using Database.Logic;
using Database.Repositories.Core;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

public class MessageRepository(
    LicenceHolderDbContext dbContext) : ReadWriteRepository<Entites.Message>(dbContext), IMessageRepository
{

    public async Task<Message> GetMessage(int organisationId, int notificationId)
    {
        return await dbContext.Messages.Where(a => a.OrganisationId == organisationId && a.Id == notificationId).FirstAsync();
    }

    public async Task<List<Message>> GetMesages(int organisationId, int skip, int take, Domain.Objects.Database.MessageType type)
    {
        return await dbContext.Messages.Where(a => a.OrganisationId == organisationId && a.MessageTypeId == (int)type).OrderByDescending(m => m.SendDateTime).Skip(skip).Take(take).ToListAsync();
    }

    public async Task<int> NumberOfMessages(int organisationId, Domain.Objects.Database.MessageType type)
    {
        return await dbContext.Messages.Where(a => a.OrganisationId == organisationId && a.MessageTypeId == (int)type).CountAsync();
    }

    public async Task MarkAsRead(int organisationId, int notificationId)
    {
        await dbContext.Messages.Where(e => e.OrganisationId == organisationId && e.Id == notificationId)
            .ExecuteUpdateAsync(b => b.SetProperty(e => e.IsRead, true));
    }

    public async Task<int> NumberOfUnReadMessages(int organisationId, Domain.Objects.Database.MessageType type)
    {
        return await dbContext.Messages.Where(a => a.OrganisationId == organisationId && a.IsRead == false && a.MessageTypeId == (int)type).CountAsync();
    }

    public async Task SendMessage(
        int organisationId,
        string body,
        string title,
        string from,
        DateTime sendDateTime,
        Domain.Objects.Database.MessageType type)
    {
        await dbContext.Messages.AddAsync(new Message
        {
            OrganisationId = organisationId,
            Body = body,
            From = from,
            Title = title,
            MessageTypeId = (int)type,
            SendDateTime = sendDateTime
        });

        await dbContext.SaveChangesAsync();
    }
}
