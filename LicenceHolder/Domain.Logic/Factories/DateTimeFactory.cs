
namespace Domain.Logic.Factories;

public class DateTimeFactory : IDateTimeFactory
{
    public DateTime Create()
    {
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
    }
}
