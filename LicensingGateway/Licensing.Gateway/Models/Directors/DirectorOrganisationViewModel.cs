using Domain.Logic.Forms.Application;
using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Directors;

public class DirectorOrganisationViewModel : ApplicationBase
{
    [BindProperty]
    public string Organisation { get; set; } = default!;

    public string[] Values = [ApplicationFormConstants.DirectorOrganisation_Value_1, ApplicationFormConstants.DirectorOrganisation_Value_2, ApplicationFormConstants.DirectorOrganisation_Value_3];
}
