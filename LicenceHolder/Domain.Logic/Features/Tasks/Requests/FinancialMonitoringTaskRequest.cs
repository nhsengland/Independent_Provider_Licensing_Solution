namespace Domain.Logic.Features.Tasks.Requests;

public record FinancialMonitoringTaskRequest
{
    public string UserOktaId { get; set; } = default!;

    public int OrganisationId { get; init; }

    public int TaskId { get; init; }
}
