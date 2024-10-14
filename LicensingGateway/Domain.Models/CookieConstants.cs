namespace Domain.Models;

public record CookieConstants
{
    public static string ApplicationName => "NHS England Independent Provider Licensing Gateway";

    public static string CookieName => "nhs-england-independent-provider-licensing-gateway-cookie-consent";

    public static string CookieValue_Accept => "Use cookies to measure my website use";

    public static string CookieValue_Decline => "Do not use cookies to measure my website use";
}
