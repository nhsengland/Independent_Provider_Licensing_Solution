using Domain.Models.Database.DTO;
using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.Directors;

public class DirectorsViewModel : ApplicationBase
{
    [BindProperty]
    public List<DirectorDTO> Directors { get; set; } = default!;

    public string GroupName { get; set; } = default!;

    public string ProviderName { get; set; } = default!;
}
