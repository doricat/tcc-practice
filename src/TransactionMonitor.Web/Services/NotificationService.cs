using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TransactionMonitor.Web.Hubs.Transaction;

namespace TransactionMonitor.Web.Services
{
    public class NotificationService : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;

        public NotificationService(ILogger<NotificationService> logger,
            IOptionsMonitor<RabbitMqOptions> rabbitMqOptionsMonitor,
            IHubContext<TransactionHub, ITransactionsClient> hubContext)
        {
            Logger = logger;
            RabbitMqOptionsMonitor = rabbitMqOptionsMonitor;
            HubContext = hubContext;
        }

        public ILogger<NotificationService> Logger { get; }

        public IOptionsMonitor<RabbitMqOptions> RabbitMqOptionsMonitor { get; }

        public IHubContext<TransactionHub, ITransactionsClient> HubContext { get; }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var options = RabbitMqOptionsMonitor.CurrentValue;
            var factory = new ConnectionFactory
            {
                HostName = options.Host,
                Port = options.Port,
                UserName = options.Username,
                Password = options.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: options.Queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            Logger.LogInformation("Waiting for messages.");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnConsumerOnReceived;
            _channel.BasicConsume(queue: options.Queue, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();

            _channel.Dispose();
            _connection.Dispose();
        }

        private async void OnConsumerOnReceived(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            Logger.LogInformation("Received: {0}", message);

            try
            {
                var model = JsonSerializer.Deserialize<TransactionViewModel>(message, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                await HubContext.Clients.All.ReceiveMessage(model);
            }
            catch (JsonException e)
            {
                // Ignored
                Logger.LogError(e, "Message deserialize failed.");
            }

            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        }
    }
}