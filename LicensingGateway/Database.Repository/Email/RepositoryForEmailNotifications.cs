using Database.Entites;
using Database.Repository.Core.ReadWrite;
using Domain.Models.Database.DTO;
using Domain.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Database.Repository.Email;
public class RepositoryForEmailNotifications(ILicensingGatewayDbContext licensingGatewayDbContext) : ReadWriteIntPkRepository<EmailNotification>(licensingGatewayDbContext), IRepositoryForEmailNotifications
{
    public async Task<List<EmailNotificationDTO>> GetUnSentEmailNotifications()
    {
        return await licensingGatewayDbContext.EmailNotification
            .Where(e => e.HasBeenSent == false)
            .Include(e => e.PreApplication)
            .Include(e => e.Application)
            .Select(e => Create(e))
        .ToListAsync();
    }

    public async Task MarkAsSent(int id)
    {
        await licensingGatewayDbContext.EmailNotification.Where(e => e.Id == id)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(e => e.HasBeenSent, true)
                    .SetProperty(e => e.DateSent, DateTime.UtcNow)
                );
    }

    private static EmailNotificationDTO Create(EmailNotification e)
    {
        var model = new EmailNotificationDTO()
        {
            Id = e.Id,
            PreApplicationId = e.PreApplicationId,
            ApplicationId = e.ApplicationId,
            ApplicationURL = e.ApplicationURL,
            Type = (Domain.Models.Database.EmailNotificationType)e.TypeId,
        };

        switch (model.Type)
        {       
            case Domain.Models.Database.EmailNotificationType.PreApplication:
                model.ApplicationReferenceId = e.PreApplication?.ReferenceId ?? throw new EmailNotificationException($"Unable to obtain pre-application reference ID {e.Id}");
                model.EmailAddress = e.PreApplication.Email ?? throw new EmailNotificationException($"Unable to obtain pre-application email address {e.Id}");
                model.Name = $"{e.PreApplication.FirstName} {e.PreApplication.LastName}";
                break;
            case Domain.Models.Database.EmailNotificationType.Application:
                model.ApplicationReferenceId = e.Application?.ReferenceId ?? throw new EmailNotificationException($"Unable to obtain main-application reference ID {e.Id}");
                model.EmailAddress = e.Application.Email ?? throw new EmailNotificationException($"Unable to obtain pre-application email address {e.Id}");
                model.Name = $"{e.Application.Forename} {e.Application.Surname}";
                break;
            case Domain.Models.Database.EmailNotificationType.MainApplicationSaveAndExit:
                model.EmailAddress = e.Application!.Email ?? throw new EmailNotificationException($"Unable to obtain pre-application email address {e.Id}");
                model.Name = $"{e.Application.Forename} {e.Application.Surname}";
                break;
            default:
                throw new NotImplementedException($"Email notification type {model.Type} not implemented");
        }

        return model;
    }

    public async Task<EmailNotificationDTO> GetEmailNotification(int id)
    {
        return await licensingGatewayDbContext.EmailNotification
            .Where(e => e.Id == id)
            .Include(e => e.PreApplication)
            .Include(e => e.Application)
            .Select(e => Create(e))
        .FirstOrDefaultAsync() ?? throw new InvalidOperationException($"EmailNotification doesn't exist: {id}");
    }
}
