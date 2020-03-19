using System;
using System.Collections.Generic;

namespace ViewModels.Shared.Order
{
    public class OrderViewModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long TransactionId { get; set; }

        public string State { get; set; }

        public DateTime CreatedAt { get; set; }

        public IList<OrderItemViewModel> Items { get; set; }
    }
}