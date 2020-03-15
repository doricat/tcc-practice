using System;
using System.IO;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;

namespace AutoCancellation.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SelfLog.Enable(Console.Error);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddUserSecrets(typeof(Program).Assembly)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

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
                })
                .UseSerilog();
    }
}