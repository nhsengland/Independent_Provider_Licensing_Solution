using Domain.Models.Database.DTO;
using Licensing.Gateway.Models.Application;

namespace Licensing.Gateway.Models.Directors.ParentCompanies;

public class ParentCompaniesViewModel : ApplicationBase
{
    public string ProviderName { get; set; } = default!;

    public List<DirectorGroupDTO> Groups { get; set; } = default!;
}
