using System;

namespace Web.Shared
{
    public class TransactionViewModel
    {
        public string ServiceName { get; set; }

        public long Id { get; set; }

        public string State { get; set; }

        public object Metadata { get; set; }

        public DateTimeOffset BeginTime { get; set; }

        public DateTimeOffset Expires { get; set; }
    }
}