namespace Domain.Objects.ViewModels.Team;

public record ManageUsersViewModel
{
    public string OrganisationName { get; set; } = default!;

    public bool IsCrsOrHaredToReplaceOrganisation { get; set; }

    public List<User> Users { get; set; } = [];

    public record User
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = default!;
        public string Lastname { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool IsVerified { get; set; }
        public string Level { get; set; } = default!;
    }
}
