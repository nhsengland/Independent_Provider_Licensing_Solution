using System.Text;

namespace Domain.Logic.Integration.CQC.Factories;
public class CQCAddressFactory : ICQCAddressFactory
{
    public string Create(
        string? addressLine1,
        string? addressLine2,
        string? townCity,
        string? region,
        string? postalCode)
    {
        var sb = new StringBuilder();

        Append(addressLine1, sb);

        Append(addressLine2, sb);

        Append(townCity, sb);

        Append(region, sb);

        Append(postalCode, sb);

        return sb.ToString();
    }

    private static void Append(string? input, StringBuilder sb)
    {
        if (!string.IsNullOrWhiteSpace(input))
        {
            if (sb.Length > 0)
            {
                sb.Append(", ");
            }

            sb.Append(input);
        }
    }
}
