using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Noteify.Data.EntityFramework;
using Noteify.Data.EntityFramework.Interfaces;
using Noteify.Data.EntityFramework.Options;
using Noteify.Data.EntityFramework.Providers;
using Noteify.Data.EntityFramework.Seed;
using Noteify.Data.Models.Auth;
using Noteify.Web.Areas.Account.Services;
using Noteify.Web.Areas.Account.Services.Interface;
using Noteify.Web.Services;
using Noteify.Web.Services.Interfaces;
using Npgsql;

namespace Noteify.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment WebHostEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        private static string GetHerokuConnectionString()
        {
            string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            var databaseUri = new Uri(connectionUrl);

            string db = databaseUri.LocalPath.TrimStart('/');
            string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

            return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
        }

        private string GetLocalConnectionString()
        {
            var builder = new NpgsqlConnectionStringBuilder(Configuration.GetConnectionString("Default_Noteify"))
            {
                Password = Configuration["DbPassword"],
                Username = Configuration["DbUserId"]
            };
            return builder.ConnectionString;
        }

        private static string GetHerokuMicrosoftClientId()
        {
            return Environment.GetEnvironmentVariable("MICROSOFT_OAUTH_CLIENTID");
        }

        private static string GetHerokuMicrosoftClientSecret()
        {
            return Environment.GetEnvironmentVariable("MICROSOFT_OAUTH_CLIENTSECRET");
        }

        private string GetLocalMicrosoftClientId()
        {
            return Configuration["Authentication:Microsoft:ClientId"];
        }

        private string GetLocalMicrosoftClientSecret()
        {
            return Configuration["Authentication:Microsoft:ClientSecret"];
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Prevent sending unessential cookies if the consent is not accepted.
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.ConsentCookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            // Register DbContext as a scoped service
            services.AddDbContext<NoteifyContext>(opts =>
                opts.UseNpgsql(WebHostEnvironment.IsDevelopment() ?
                    GetLocalConnectionString() :
                    GetHerokuConnectionString()));

            // Identity
            services.AddIdentity<User, IdentityRole<string>>()
                .AddEntityFrameworkStores<NoteifyContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opts =>
            {
                opts.User.AllowedUserNameCharacters = null;
            });

            services.ConfigureApplicationCookie(opts =>
            {
                opts.LoginPath = "/Account/Login";
                opts.LogoutPath = "/Account/Logout";
                opts.AccessDeniedPath = "/Error/403";

                // Mark application cookie as unessential to prevent transmission if cookie consent is rejected.
                opts.Cookie.IsEssential = false;
            });

            // Microsoft OAuth 2.0
            services.AddAuthentication().AddMicrosoftAccount("MicrosoftProv", microsoftOptions =>
            {
                microsoftOptions.ClientId = WebHostEnvironment.IsDevelopment() ?
                    GetLocalMicrosoftClientId() :
                    GetHerokuMicrosoftClientId();

                microsoftOptions.ClientSecret = WebHostEnvironment.IsDevelopment() ?
                    GetLocalMicrosoftClientSecret() :
                    GetHerokuMicrosoftClientSecret();

                microsoftOptions.UsePkce = true;

                // Mark oauth cookie as unessential to prevent transmission if cookie consent is rejected.
                microsoftOptions.CorrelationCookie.IsEssential = false;

                // Least privilege principle
                microsoftOptions.Scope.Clear();
                microsoftOptions.Scope.Add("user.read");

                microsoftOptions.Events = new OAuthEvents
                {
                    OnAccessDenied = ctx =>
                    {
                        ctx.Response.Redirect("/Account/MicrosoftError/AccessDenied");
                        ctx.HandleResponse();
                        return Task.CompletedTask;
                    },
                    OnRemoteFailure = ctx =>
                    {
                        // The user has refused access to his Microsoft account
                        ctx.Response.Redirect("/Account/MicrosoftError/PermissionDenied");
                        ctx.HandleResponse();
                        return Task.CompletedTask;
                    }
                };

            });

            services.AddControllersWithViews(opts =>
            {
                // Globally allow only authenticated users
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                opts.Filters.Add(new AuthorizeFilter(policy));
            });

            // Heroku forwards requests to the Docker container via HTTP.
            // Therefore, internal redirection to HTTPS.
            // Necessary for Mircrosoft OAuth 2.0!
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto;

                // Prevent eternal redirection to HTTPS.
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            if (WebHostEnvironment.IsProduction())
            {
                services.AddHttpsRedirection(opt => opt.HttpsPort = 443);
            }
            else
            {
                services.AddHttpsRedirection(opt => opt.HttpsPort = 5001);
            }

            // Enable access to the application user outside the controller
            services.AddHttpContextAccessor();

            // Provide UserId where dependency injection of HttpContext is not possible.
            services.AddScoped<UserIdProvider>();

            // UnitOfWork/Service Layer
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IManageService, ManageService>();
            services.AddScoped<ErrorService>();

            // Register seed object for the dev. test user
            services.Configure<SeedUserOptions>(SeedUserOptions.NormalUser,
                                   Configuration.GetSection("SeedUserDev:NormalUser"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Apply migrations and seed database for test purpose
                serviceProvider.GetService<NoteifyContext>()
                    .EnsureDbCreatedAndSeededDev(
                        serviceProvider.GetRequiredService<UserManager<User>>(),
                        serviceProvider.GetRequiredService<IOptionsSnapshot<SeedUserOptions>>());
            }
            else
            {
                // Apply migrations only
                serviceProvider.GetService<NoteifyContext>()
                    .EnsureDbCreatedProd();

                app.UseExceptionHandler(errorHandler =>
                {
                    errorHandler.Run(async context =>
                    {
                        await Task.Run(() =>
                        {
                            context.Response.Redirect("/Error/500");
                        });
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Use forwarded headers especially for Heroku
            app.UseForwardedHeaders();
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            // Cookie consent
            app.UseCookiePolicy();

            app.UseRouting();

            // Identity
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "AccountArea",
                    areaName: "Account",
                    pattern: "Account/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
