﻿using System.Text.Json;
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
                conn.Notification += (o, e) =>
                {
                    Logger.LogInformation("Received notification {0}", e.Payload);

                    var model = JsonSerializer.Deserialize<TransactionViewModel>(e.Payload, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    MessageSender.SendAsync(model);
                };

                var channelName = Configuration.GetValue<string>("ChannelName");
                using (var cmd = new NpgsqlCommand($"listen {channelName}", conn))
                {
                    Logger.LogInformation("Listen {channelName}", channelName);

                    await cmd.ExecuteNonQueryAsync(stoppingToken);
                }

                await conn.WaitAsync(stoppingToken);
            }
        }
    }
}