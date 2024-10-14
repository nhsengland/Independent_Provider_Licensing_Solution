namespace Domain.Logic.Integration.CQC.API.Models;
public record ProvidersInputModel
{
    public int PageNumber { get; init; }

    public Guid InstanceId { get; init; } = default!;
}
