using Database.Repositories.User;
using Domain.Logic.Extensions;
using Domain.Objects.Exceptions;
using System.Security.Claims;

namespace Licence.Holder.Application.Middleware;

public class HandleUserMiddleware(
    RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(
        HttpContext context,
        IRepositoryForUser repositoryForUser,
        ITypeConverter typeConverter)
    {
        if (context.User.Identity!.IsAuthenticated && context.Request.Path != "/Account/AccessDenied")
        {
            var oktaId = context.User.FindFirstValue("sub") ?? string.Empty;

            var firstName = context.User.FindFirstValue("given_name") ?? throw new HandleUserMiddlewareException($"Unable to get idp for: {context.User}");

            var lastName = context.User.FindFirstValue("family_name") ?? string.Empty;

            var email = context.User.FindFirstValue("email") ?? string.Empty;

            var verified = typeConverter.Convert(context.User.FindFirstValue("email_verified"));

            var userIsDeleted = await repositoryForUser.UpdateUserWhenLoggingInAsync(oktaId, firstName, lastName, email, verified);

            if (userIsDeleted && context.Request.Path != "/Account/SignOut")
            {
                context.Response.Redirect("/Account/AccessDenied");
                return;
            }
        }

        await _next(context);
    }
}
