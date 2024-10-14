using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Directors;

public class DirectorNameViewModel : ApplicationBase
{
    [BindProperty]
    public string Forename { get; set; } = default!;

    [BindProperty]
    public string Surname { get; set; } = default!;
}
