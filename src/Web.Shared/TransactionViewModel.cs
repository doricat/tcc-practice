using System;

namespace Web.Shared
{
    public class TransactionViewModel
    {
        public string ServiceName { get; set; }

        public long Id { get; set; }

        public string State { get; set; }

        public object Metadata { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime Expires { get; set; }
    }
}