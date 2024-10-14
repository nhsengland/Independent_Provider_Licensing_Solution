namespace Database.Entites.Factories;

public interface IEmailNotificationFactory
{
    EmailNotification Create(int userId, int typeId, int requestedById);
}
