using Domain.Objects.ViewModels.Dashboard;

namespace Domain.Logic.Features.Dashboard;

public interface IDashboardViewModelHandler
{
    Task<DashboardViewModel> Get(GetDashboardViewModelQuery request, CancellationToken cancellationToken);
}
