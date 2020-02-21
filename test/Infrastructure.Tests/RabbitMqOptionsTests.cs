using NUnit.Framework;

namespace Infrastructure.Tests
{
    public class RabbitMqOptionsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var options = RabbitMqOptions.Parse(
                "Host=192.168.1.5; Queue=queue-name; Username=username; Password=password; Port=5672; Exchange=exchange-name; Arguments=x-message-ttl:5000, x-dead-letter-exchange:exchange-ttl-to");
            Assert.AreEqual(options.Host, "192.168.1.5");
            Assert.AreEqual(options.Port, 5672);
            Assert.AreEqual(options.Queue, "queue-name");
            Assert.AreEqual(options.Exchange, "exchange-name");
            Assert.AreEqual(options.Username, "username");
            Assert.AreEqual(options.Password, "password");
            Assert.AreEqual(options.Arguments.Count, 2);
            Assert.AreEqual(options.Arguments["x-message-ttl"], 5000);
            Assert.AreEqual(options.Arguments["x-dead-letter-exchange"], "exchange-ttl-to");
        }
    }
}