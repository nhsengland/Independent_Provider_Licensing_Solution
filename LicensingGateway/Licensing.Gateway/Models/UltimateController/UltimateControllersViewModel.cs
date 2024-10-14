using Domain.Models.Database.DTO;
using Licensing.Gateway.Models.Application;

namespace Licensing.Gateway.Models.UltimateController;

public class UltimateControllersViewModel : ApplicationBase
{
    public List<UltimateControllerDTO> UltimateControllers { get; set; } = [];
}
