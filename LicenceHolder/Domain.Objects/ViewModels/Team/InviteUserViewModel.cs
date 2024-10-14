using Domain.Objects.Database;

namespace Domain.Objects.ViewModels.Team;

public record InviteUserViewModel
{
    public string OrganisationName { get; init; } = default!;

    public bool IsCrsOrHardToReplaceOrganisation { get; set; }

    public UserRole UserRole { get; set; }
}
