using Domain.Logic.Forms.PreApplication;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.PreApplication;

public class ConfirmCQCProviderInformationViewModel : ApplicationBase
{
    [BindProperty]
    public string CQCInformationIsCorrect { get; set; } = default!;

    public string[] CQCInformationIsCorrectValues = [PreApplicationFormConstants.Yes, PreApplicationFormConstants.No];

    public string CQCProviderID { get; set; } = default!;

    public string CQCProvider_Name { get; set; } = default!;

    public string CQCProvider_Address { get; set; } = default!;

    public string CQCProvider_PhoneNumber { get; set; } = default!;
}
