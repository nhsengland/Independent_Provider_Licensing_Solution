using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Okta.AspNetCore;
using Domain.Logic.Extensions;
using Database.Extensions;
using Licence.Holder.Application.Middleware;

namespace Licence.Holder.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddApplicationInsightsTelemetry();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            })
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/AccountAccessDenied";
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddOktaMvc(new OktaMvcOptions
            {
                OktaDomain = builder.Configuration.GetValue<string>("Okta:Domain"),
                AuthorizationServerId = null,
                ClientId = builder.Configuration.GetValue<string>("Okta:ClientId"),
                ClientSecret = builder.Configuration.GetValue<string>("Okta:ClientSecret"),
                Scope = ["openid", "profile", "groups", "email"],
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
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(builder.Configuration.GetValue<int>("Session:IdleTimeoutFromMinutes"));
            });

            builder.Services.AddDatabaseDependencies(builder.Configuration);

            builder.Services.AddDomainLogicDependencies(builder.Configuration);

            var app = builder.Build();

            app.UseConventionalMiddleware();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.None
                //HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                //MinimumSameSitePolicy = SameSiteMode.Strict
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.UseMiddleware<HandleUserMiddleware>();

            app.MapControllerRoute(
                name: "AccountSignOut",
                pattern: "accountsignout",
                new { Controller = "Account", Action = "SignOut" });

            app.MapControllerRoute(
                name: "AccountAccessDenied",
                pattern: "AccountAccessDenied",
                new { Controller = "Account", Action = "AccessDenied" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "Messages",
                pattern: "{controller=Messages}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "License",
                pattern: "{controller=License}/{action=Index}/{id?}");

            app.UseStatusCodePagesWithRedirects("/home/page-not-found");

            // sets headers
            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("X-Frame-Options", "Deny");
            //    context.Response.Headers.Add("Cross-Origin-Resource-Policy", "same-origin");
            //    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
            //    context.Response.Headers.Add("Content-Security-Policy", "frame-src 'none'; frame-ancestors 'none'");
            //    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            //    context.Response.Headers.Add("Permissions-Policy", "cross-origin-isolated=(), accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
            //    context.Response.Headers.Add("Strict-Transport-Security", "max-age = 31536000; includeSubDomains");
            //    context.Response.Headers.Add("Cache-Control", "no-store, max-age=0");
            //    await next();
            //});

            app.Run();
        }
    }
}
