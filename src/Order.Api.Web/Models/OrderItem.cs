namespace Order.Api.Web.Models
{
    public class OrderItem
    {
        public long Id { get; set; }

        public long OrderId { get; set; }

        public long ProductId { get; set; }

        public decimal Price { get; set; }

        public int Qty { get; set; }

        public virtual Order Order { get; set; }
    }
}