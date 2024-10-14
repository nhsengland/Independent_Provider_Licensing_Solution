
using System.Globalization;

namespace Domain.Logic.Integration.CQC.API.Factories;
public class CQCDateFactory : ICQCDateFactory
{
    public string Create(int addDays)
    {
        return DateTime.UtcNow.AddDays(addDays).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);
    }
}
