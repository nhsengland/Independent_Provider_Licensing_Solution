using Domain.Logic.Forms.Application;
using Domain.Models.Database.DTO;
using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Directors;

public class DirectorsSatisfyG3FitAndProperRequirementsViewModel : ApplicationBase
{
    public List<DirectorDTO> Directors { get; set; } = default!;

    [BindProperty]
    public string DirectorsFitAndProper { get; set; } = default!;

    public string[] Values = [ApplicationFormConstants.Yes, ApplicationFormConstants.No];

    [BindProperty]
    public string? IfNoWhy { get; set; }
}
