using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ViewModels.Shared;

namespace AutoCancellation.Service
{
    public class CancellationService : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;

        public CancellationService(ILogger<CancellationService> logger,
            IConfiguration configuration,
            IOptionsMonitor<RabbitMqOptions> rabbitMqOptionsMonitor,
            IHttpClientFactory httpClientFactory)
        {
            Logger = logger;
            Configuration = configuration;
            RabbitMqOptionsMonitor = rabbitMqOptionsMonitor;
            HttpClientFactory = httpClientFactory;
        }

        public ILogger<CancellationService> Logger { get; }

        public IConfiguration Configuration { get; }

        public IOptionsMonitor<RabbitMqOptions> RabbitMqOptionsMonitor { get; }

        public IHttpClientFactory HttpClientFactory { get; }

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
                var item = JsonSerializer.Deserialize<Link>(message /*, new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}*/);
                var client = HttpClientFactory.CreateClient();
                var resp = await client.DeleteAsync(item.Uri);
                if (resp.IsSuccessStatusCode)
                {
                    Logger.LogInformation("Canceled: {0}", item.Uri);
                }
                else if (resp.StatusCode == HttpStatusCode.NotFound)
                {
                    Logger.LogInformation("Confirmed or Canceled: {0}", item.Uri);
                }
                else
                {
                    Logger.LogError($"{item.Uri}, {resp.StatusCode}, \r\n{await resp.Content.ReadAsStringAsync()}");
                }
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