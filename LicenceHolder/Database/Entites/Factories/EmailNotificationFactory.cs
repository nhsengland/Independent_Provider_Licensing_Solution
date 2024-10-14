namespace Database.Entites.Factories;

public class EmailNotificationFactory : IEmailNotificationFactory
{
    public EmailNotification Create(
        int userId,
        int typeId,
        int requestedById)
    {
        return new EmailNotification
        {
            UserId = userId,
            TypeId = typeId,
            HasBeenSent = false,
            RequestedById = requestedById,
        };
    }
}
