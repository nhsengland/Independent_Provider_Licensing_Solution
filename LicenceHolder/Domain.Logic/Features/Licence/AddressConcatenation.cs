using System.Text;

namespace Domain.Logic.Features.Licence;

public class AddressConcatenation : IAddressConcatenation
{
    public string Concat(string line1, string line2, string townOrCity, string county, string postcode)
    {
        var concatenatedAddress = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(line1))
        {
            concatenatedAddress.Append(line1.Trim()).Append(", ");
        }

        if (!string.IsNullOrWhiteSpace(line2))
        {
            concatenatedAddress.Append(line2.Trim()).Append(", ");
        }

        if (!string.IsNullOrWhiteSpace(townOrCity))
        {
            concatenatedAddress.Append(townOrCity.Trim()).Append(", ");
        }

        if (!string.IsNullOrWhiteSpace(county))
        {
            concatenatedAddress.Append(county.Trim()).Append(", ");
        }

        if (!string.IsNullOrWhiteSpace(postcode))
        {
            concatenatedAddress.Append(postcode.Trim());
        }

        return concatenatedAddress.ToString();
    }
}
