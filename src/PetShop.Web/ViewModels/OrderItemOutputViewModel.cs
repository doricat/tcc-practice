using ViewModels.Shared.Order;

namespace PetShop.Web.ViewModels
{
    public class OrderItemOutputViewModel
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Qty { get; set; }

        public static OrderItemOutputViewModel FromApiModel(OrderItemViewModel model)
        {
            return new OrderItemOutputViewModel
            {
                Id = model.Id.ToString(),
                ProductId = model.ProductId.ToString(),
                Name = model.Name,
                Image = model.Image,
                Price = model.Price,
                Qty = model.Qty
            };
        }
    }
}