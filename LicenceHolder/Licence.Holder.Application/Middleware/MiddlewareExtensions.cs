namespace Licence.Holder.Application.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseConventionalMiddleware(
        this IApplicationBuilder app)
        => app.UseMiddleware<HandleUserMiddleware>();
}
