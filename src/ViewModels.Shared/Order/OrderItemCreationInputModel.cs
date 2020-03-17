namespace ViewModels.Shared.Order
{
    public class OrderItemCreationInputModel
    {
        public long ProductId { get; set; }

        public int Qty { get; set; } = 1;

        public decimal Price { get; set; }
    }
}