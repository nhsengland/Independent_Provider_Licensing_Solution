using Database.DI;
using Database.LicenceHolder.Readonly.DI;
using Database.Repository.Extensions;
using Domain.Logic.Extensions;
using Licensing.Gateway.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Okta.AspNetCore;

namespace Licensing.Gateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddApplicationInsightsTelemetry();

        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddRouting(option =>
        {
            option.LowercaseUrls = true;
        });

        var oktaSection = builder.Configuration.GetSection("Okta");

        var oktaMvcOptions = new OktaMvcOptions()
        {
            OktaDomain = oktaSection.GetValue<string>("Domain"),
            ClientId = oktaSection.GetValue<string>("ClientId"),
            ClientSecret = oktaSection.GetValue<string>("ClientSecret"),
            Scope = ["openid", "profile", "email"],
            OpenIdConnectEvents = new OpenIdConnectEvents
            {
                OnRemoteFailure = context =>
                {
                    if (context.Failure.Message.Contains("access_denied"))
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/Account/AccessDenied?message=User is not assigned to the client application.");
                    }
                    else
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/Account/AccessDenied?message=An unexpected error occurred.");
                    }

                    return Task.CompletedTask;
                }
            }
        };

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OktaDefaults.MvcAuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.SlidingExpiration = true;
            options.AccessDeniedPath = "/Account/AccessDenied";
        })
        .AddOktaMvc(oktaMvcOptions);

        builder.Services.AddControllersWithViews();

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(builder.Configuration.GetValue<int>("Session:IdleTimeoutFromMinutes"));
        });

        LicenceGatewayDependencyRegistration.Add(builder.Services, builder.Configuration);

        ReadonlyDatabaseLicenceHolderDependencyRegistration.Add(builder.Services, builder.Configuration);

        builder.Services.Add();

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseSession();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.UseStatusCodePagesWithRedirects("/home/page-not-found");

        app.Run();
    }
}
