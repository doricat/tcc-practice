namespace ViewModels.Shared.Order
{
    public class OrderItemViewModel
    {
        public long Id { get; set; }

        public long ProductId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Qty { get; set; }
    }
}