using Domain.Objects.Database;

namespace Domain.Logic.Features.Team.Maps;

public interface IAccessLevelMapper
{
    public UserRole MapToUserRole(string accessLevel);

    public string MapToAccessLevel(int userRole);
}
