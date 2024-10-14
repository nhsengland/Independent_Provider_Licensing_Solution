namespace Domain.Logic.Features.Tasks.Requests;

public record AnnualCertificateTaskRequest
{
    public string UserOktaId { get; set; } = default!;

    public int TaskId { get; init; }

    public int LicenseId { get; init; }
}
