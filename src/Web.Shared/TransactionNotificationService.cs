using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Web.Shared
{
    public class TransactionNotificationService : BackgroundService
    {
        public TransactionNotificationService(ILogger<TransactionNotificationService> logger,
            IConfiguration configuration,
            TransactionRabbitMqMessageSender messageSender)
        {
            Logger = logger;
            Configuration = configuration;
            MessageSender = messageSender;
        }

        public ILogger<TransactionNotificationService> Logger { get; }

        public IConfiguration Configuration { get; }

        public TransactionRabbitMqMessageSender MessageSender { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var conn = new NpgsqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                await conn.OpenAsync(stoppingToken);
                conn.Notification += async (o, e) =>
                {
                    Logger.LogInformation("Received notification {0}", e.Payload);

                    try
                    {
                        var model = JsonSerializer.Deserialize<TransactionViewModel>(e.Payload, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                        model.ServiceName = Configuration.GetValue<string>("ServiceName");
                        await MessageSender.SendAsync(model);
                    }
                    catch (JsonException ex)
                    {
                        Logger.LogError(ex, "反序列化失败 {json}", e.Payload);
                        // Ignored
                    }
                };

                var channelName = Configuration.GetValue<string>("ChannelName");
                using (var cmd = new NpgsqlCommand($"listen {channelName}", conn))
                {
                    Logger.LogInformation("Listen {channelName}", channelName);

                    await cmd.ExecuteNonQueryAsync(stoppingToken);
                }

                while (!stoppingToken.IsCancellationRequested)
                {
                    await conn.WaitAsync(stoppingToken);
                }
            }
        }
    }
}