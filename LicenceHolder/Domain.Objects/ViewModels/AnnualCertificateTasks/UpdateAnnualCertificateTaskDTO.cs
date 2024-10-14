namespace Domain.Objects.ViewModels.AnnualCertificateTasks;

public record UpdateAnnualCertificateTaskDTO
{
    public string LicenceName { get; init; } = default!;

    public int LicenceId { get; init; }
}
