namespace Domain.Models.Exceptions;
public class EmailNotificationException : Exception
{
    public EmailNotificationException(string message) : base(message)
    {
    }
}
