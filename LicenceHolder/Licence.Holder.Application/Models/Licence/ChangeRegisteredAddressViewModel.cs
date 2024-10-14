namespace Licence.Holder.Application.Models.Licence;

public class ChangeRegisteredAddressViewModel
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = default!;

    public string Address_Line_1 { get; set; } = string.Empty;
    public string Address_Line_2 { get; set; } = string.Empty;
    public string Address_TownOrCity { get; set; } = string.Empty;
    public string Address_County { get; set; } = string.Empty;
    public string Address_Postcode { get; set; } = string.Empty;

    public bool ValidationFailure { get; set; } = false;
}
