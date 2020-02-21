using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public class RabbitMqOptions
    {
        private static readonly IDictionary<string, Action<string, RabbitMqOptions>> Actions =
            new Dictionary<string, Action<string, RabbitMqOptions>>(StringComparer.OrdinalIgnoreCase)
            {
                {"Host", (s, options) => options.Host = s},
                {"Port", (s, options) => options.Port = int.Parse(s)},
                {"Queue", (s, options) => options.Queue = s},
                {"Exchange", (s, options) => options.Exchange = s},
                {"Username", (s, options) => options.Username = s},
                {"Password", (s, options) => options.Password = s},
                {
                    "Arguments", (s, options) =>
                    {
                        options.Arguments = s.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Split(':', StringSplitOptions.RemoveEmptyEntries)).ToDictionary(x => x[0].Trim(), x =>
                            {
                                var v = x[1].Trim();
                                if (int.TryParse(v, out var i))
                                {
                                    return i as object;
                                }

                                return v as object;
                            });
                    }
                },
                {"RouteKey", (s, options) => options.RouteKey = s}
            };

        public string Host { get; set; }

        public int Port { get; set; }

        public string Queue { get; set; }

        public string Exchange { get; set; }

        public string RouteKey { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public IDictionary<string, object> Arguments { get; set; }

        public string ConnectionString { get; set; }

        public static RabbitMqOptions Parse(string connectionString)
        {
            var options = new RabbitMqOptions();
            Parse(connectionString, options);

            return options;
        }

        public static void Parse(string connectionString, RabbitMqOptions options)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            var array = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

            foreach (var s in array)
            {
                var kv = s.Split('=', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                if (kv.Length > 2)
                {
                    throw new InvalidOperationException();
                }

                if (Actions.ContainsKey(kv[0]))
                {
                    Actions[kv[0]](kv[1], options);
                }
            }

            options.ConnectionString = connectionString;
        }
    }
}
