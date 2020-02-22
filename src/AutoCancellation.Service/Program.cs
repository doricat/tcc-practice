using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoCancellation.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<CancellationService>();
                    services.AddHttpClient();
                    services.Configure<RabbitMqOptions>(options =>
                        RabbitMqOptions.Parse(hostContext.Configuration.GetConnectionString("RabbitMqConnection"), options));
                });
    }
}