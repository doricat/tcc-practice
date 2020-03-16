using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Web.Shared;

namespace Product.Api.Web
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
            services.AddHostedService<TransactionNotificationService>();
            services.Configure<RabbitMqOptions>("Transaction",
                options => RabbitMqOptions.Parse(Configuration.GetConnectionString("TransactionRabbitMqConnection"), options));
            services.Configure<RabbitMqOptions>("CancellationTask",
                options => RabbitMqOptions.Parse(Configuration.GetConnectionString("CancellationTaskRabbitMqConnection"), options));
            services.AddSingleton<CancellationTaskRabbitMqMessageSender>();
            services.AddSingleton<TransactionRabbitMqMessageSender>();

            services.AddSingleton(provider => new IdentityGeneratorOptions { InstanceTag = 0 });
            services.AddSingleton<IdentityGenerator>();
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
                endpoints.MapControllers();
            });
        }
    }
}