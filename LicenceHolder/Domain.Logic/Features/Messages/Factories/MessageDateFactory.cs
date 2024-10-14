namespace Domain.Logic.Features.Messages.Factories;

public class MessageDateFactory : IMessageDateFactory
{
    public string CreateDate(DateTime date)
    {
        if (date.Date == DateTime.Today)
        {
            return date.ToString("HH:mm");
        }

        if (date.Date == DateTime.Today.AddDays(-1))
        {
            return "Yesterday";
        }

        if (date.Date > DateTime.Today.AddDays(-7))
        {
            return date.ToString("dddd");
        }

        return date.ToString("dd-MM-yyyy");
    }
}
