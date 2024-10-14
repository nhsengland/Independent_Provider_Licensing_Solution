using Domain.Logic.Forms.PreApplication;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.PreApplication;

public class CQCRegulatedActivitesViewModel : ApplicationBase
{

    [BindProperty]
    public string Confirmation { get; set; } = default!;

    public string[] Values = [PreApplicationFormConstants.Yes, PreApplicationFormConstants.No];

    public string[] Activities { get; set; } = default!;

    public string ProviderID { get; set; } = default!;

    public string ProviderName { get; set; } = default!;
}
