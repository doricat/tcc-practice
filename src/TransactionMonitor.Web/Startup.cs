using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TransactionMonitor.Web.Hubs.Transaction;
using TransactionMonitor.Web.Services;

namespace TransactionMonitor.Web
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
            services.AddControllers();
            services.AddHealthChecks();
            services.AddSignalR();
            services.AddHostedService<NotificationService>();
            services.Configure<RabbitMqOptions>(options => RabbitMqOptions.Parse(Configuration.GetConnectionString("RabbitMqConnection"), options));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/_health");
                endpoints.MapHub<TransactionHub>("/transaction_hub");
                endpoints.MapControllers();
            });
        }
    }
}