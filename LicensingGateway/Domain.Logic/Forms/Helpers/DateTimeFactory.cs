
namespace Domain.Logic.Forms.Helpers;

public class DateTimeFactory : IDateTimeFactory
{
    public DateTime Create()
    {
        // Get the local time zone
        var localTimeZone = TimeZoneInfo.Local;

        // Convert the current UTC time to the local time zone
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimeZone);
    }
}
