using Domain.Logic.Forms.Application;
using Domain.Models.Database.DTO;
using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Directors;

public class DirectorRemoveConfirmViewModel : ApplicationBase
{
    [BindProperty]
    public string Confirmation { get; set; } = default!;

    public DirectorDTO Director { get; set; } = default!;

    public string[] Values = [ApplicationFormConstants.Yes, ApplicationFormConstants.No];
}
