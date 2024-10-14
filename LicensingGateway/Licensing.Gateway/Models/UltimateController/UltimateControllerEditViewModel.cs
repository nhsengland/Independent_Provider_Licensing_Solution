using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.UltimateController;

public class UltimateControllerEditViewModel : ApplicationBase
{
    [BindProperty]
    public int Id { get; set; } = default!;
}
