using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Directors;

public class DirectorsEditViewModel : ApplicationBase
{
    [BindProperty]
    public int Id { get; set; } = default!;
}
