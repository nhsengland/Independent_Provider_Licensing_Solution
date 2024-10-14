namespace Domain.Objects.ViewModels.FinancialMonitoringTasks;

public record UpdateFinancialMonitoringTaskDTO
{
    public string LicenceName { get; init; } = default!;

    public int LicenceId { get; init; }
}
