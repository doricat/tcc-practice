using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Web.Shared;

namespace PetShop.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHealthChecks();
            services.AddHttpClient();

            services.Configure<OidcOptions>(options => Configuration.GetSection("OidcOptions").Bind(options));

            var apiOptions = new OidcApiOptions();
            Configuration.GetSection("OidcApiOptions").Bind(apiOptions);
            services.AddAuthentication("token")
                .AddIdentityServerAuthentication("token", options =>
                {
                    options.Authority = apiOptions.Authority;
                    options.RequireHttpsMetadata = false;

                    options.ApiName = apiOptions.ApiName;
                    options.ApiSecret = apiOptions.Secret;

                    options.JwtBearerEvents = new JwtBearerEvents
                    {
                        OnTokenValidated = e =>
                        {
                            var jwt = (JwtSecurityToken) e.SecurityToken;
                            var type = jwt.Header.Typ;

                            if (!string.Equals(type, "at+jwt", StringComparison.Ordinal))
                            {
                                e.Fail("JWT is not an access token");
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddSingleton(provider => new IdentityGeneratorOptions { InstanceTag = 3 });
            services.AddSingleton<IdentityGenerator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSerilogRequestLogging();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/_health");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            if (env.IsStaging())
            {
                app.UseSpa(spa => { spa.UseProxyToSpaDevelopmentServer("http://localhost:3001"); });
            }
        }
    }
}