using Domain.Logic.Forms.Application;
using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Directors;

public class DirectorRoleViewModel : ApplicationBase
{
    [BindProperty]
    public string Role { get; set; } = default!;

    public string[] Values = [ApplicationFormConstants.DirectorRoles_Value_1, ApplicationFormConstants.DirectorRoles_Value_2];
}
