namespace Domain.Objects.ViewModels.Tasks;

public record TaskOverviewViewModel : TaskViewModel
{
    public string Name { get; init; } = default!;

    public string LicenseNumber { get; init; } = default!;

    public bool OrganisationIsCrsOrHardToReplace { get; init; }
}
