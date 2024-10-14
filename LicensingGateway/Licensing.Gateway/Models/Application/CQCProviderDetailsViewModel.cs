using Domain.Logic.Forms.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Application;

public class CQCProviderDetailsViewModel : ApplicationBase
{
    [BindProperty]
    public string CQCProviderID { get; set; } = default!;

    [BindProperty]
    public string Name { get; set; } = default!;

    [BindProperty]
    public string Address { get; set; } = default!;

    [BindProperty]
    public string PhoneNumber { get; set; } = default!;

    [BindProperty]
    public string WebsiteURL { get; set; } = default!;

    [BindProperty]
    public string CQCInformationIsCorrect { get; set; } = default!;

    public string[] CQCInformationIsCorrectValues = [ApplicationFormConstants.Yes, ApplicationFormConstants.No];

    public bool IsCrsOrHtr { get; set; }
}
