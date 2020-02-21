using System.Threading;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Web.Shared
{
    public class TransactionNotificationService : BackgroundService
    {
        public TransactionNotificationService(ILogger<TransactionNotificationService> logger,
            IConfiguration configuration,
            IOptionsMonitor<RabbitMqOptions> rabbitMqOptionsMonitor)
        {
            Logger = logger;
            Configuration = configuration;
            RabbitMqOptionsMonitor = rabbitMqOptionsMonitor;
        }

        public ILogger<TransactionNotificationService> Logger { get; }

        public IConfiguration Configuration { get; }

        public IOptionsMonitor<RabbitMqOptions> RabbitMqOptionsMonitor { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var conn = new NpgsqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                await conn.OpenAsync(stoppingToken);
                conn.Notification += (o, e) =>
                {
                    Logger.LogDebug("Received notification {0}", e.Payload);

                    // TODO 发送到消息队列
                };

                var channelName = Configuration.GetValue<string>("ChannelName");
                using (var cmd = new NpgsqlCommand($"listen {channelName}", conn))
                {
                    await cmd.ExecuteNonQueryAsync(stoppingToken);
                }

                await conn.WaitAsync(stoppingToken);
            }
        }
    }
}