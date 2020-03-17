using System.Collections.Generic;

namespace ViewModels.Shared.Order
{
    public class OrderCreationInputModel
    {
        public long UserId { get; set; }

        public long TransactionId { get; set; }

        public int Timeout { get; set; } = 5000; // ms

        public IList<OrderItemCreationInputModel> Items { get; set; }
    }
}