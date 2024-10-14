using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Directors;

public class DirectorDateOfBirthViewModel : ApplicationBase
{
    [BindProperty]
    public int? Day { get; set; } = default!;
    [BindProperty]
    public int? Month { get; set; } = default!;
    [BindProperty]
    public int? Year { get; set; } = default!;

    public bool? IsValidDate { get; set; } = default!;
}
