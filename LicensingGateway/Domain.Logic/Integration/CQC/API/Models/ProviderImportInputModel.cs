namespace Domain.Logic.Integration.CQC.API.Models;
public record ProviderImportInputModel
{
    public string ProviderID { get; init; } = default!;

    public Guid InstanceId { get; init; } = default!;
}
