using System.Collections.Generic;

namespace ViewModels.Shared.Order
{
    public class OrderCreationInputModel
    {
        public long UserId { get; set; }

        public long TransactionId { get; set; }

        public IList<OrderItemCreationInputModel> Items { get; set; }
    }
}