using Domain.Objects;
using Domain.Objects.Database;

namespace Domain.Logic.Features.Team.Maps;

public class AccessLevelMapper : IAccessLevelMapper
{
    public string MapToAccessLevel(int userRole)
    {
        return userRole == (int)UserRole.Level2 ? PageConstants.Yes : PageConstants.No;
    }

    public UserRole MapToUserRole(string accessLevel)
    {
        return accessLevel == PageConstants.Yes ? UserRole.Level2 : UserRole.Level1;
    }
}
