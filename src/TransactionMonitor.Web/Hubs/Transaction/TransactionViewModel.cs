using System;

namespace TransactionMonitor.Web.Hubs.Transaction
{
    public class TransactionViewModel
    {
        public string ServiceName { get; set; }

        public long Id { get; set; }

        public string Sid => Id.ToString();

        public string State { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime Expires { get; set; }
    }
}