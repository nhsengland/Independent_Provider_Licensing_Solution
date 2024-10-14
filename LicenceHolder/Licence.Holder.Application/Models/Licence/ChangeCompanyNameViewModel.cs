namespace Licence.Holder.Application.Models.Licence;

public class ChangeCompanyNameViewModel
{
    public int CompanyId { get; set; }

    public string CurrentCompanyName { get; set; } = default!;

    public string CompanyName { get; set; } = default!;

    public bool ValidationFailure { get; set; } = false;
}
