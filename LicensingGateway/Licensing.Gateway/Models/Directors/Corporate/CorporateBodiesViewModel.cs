using Domain.Models.Database.DTO;
using Licensing.Gateway.Models.Application;

namespace Licensing.Gateway.Models.Directors.Corporate;

public class CorporateBodiesViewModel : ApplicationBase
{
    public string ProviderName { get; set; } = default!;

    public List<DirectorGroupDTO> Groups { get; set; } = default!;
}
