using Domain.Logic.Forms.PreApplication;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.PreApplication;

public class IsRegisteredWithCQCViewModel : ApplicationBase
{
    [BindProperty]
    public string IsCQCRegistered { get; set; } = default!;

    public string[] IsCQCRegisteredValues = [PreApplicationFormConstants.Yes, PreApplicationFormConstants.No];
}
