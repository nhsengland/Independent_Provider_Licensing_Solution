using Domain.Objects;

namespace Licence.Holder.Application.Models.Cookie;

public class CookieViewModel
{
    public string SelectedValue { get; set; } = string.Empty;

    public string[] Values { get; set; } = [CookieConstants.CookieValue_Accept, CookieConstants.CookieValue_Decline];
}
