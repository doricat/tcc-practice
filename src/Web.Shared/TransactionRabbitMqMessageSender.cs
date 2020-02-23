using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Web.Shared
{
    public class TransactionRabbitMqMessageSender : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public TransactionRabbitMqMessageSender(ILogger<TransactionRabbitMqMessageSender> logger,
            IOptionsMonitor<RabbitMqOptions> rabbitMqOptionsMonitor)
        {
            Logger = logger;
            RabbitMqOptionsMonitor = rabbitMqOptionsMonitor;

            var options = RabbitMqOptionsMonitor.Get("Transaction");
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
                autoDelete: false
            );
        }

        public ILogger<TransactionRabbitMqMessageSender> Logger { get; }

        public IOptionsMonitor<RabbitMqOptions> RabbitMqOptionsMonitor { get; }

        public Task SendAsync(object data)
        {
            var options = RabbitMqOptionsMonitor.CurrentValue;
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true
                }));

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: "",
                routingKey: options.Queue,
                basicProperties: properties,
                body: body);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}