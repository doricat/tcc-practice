using ViewModels.Shared.Product;

namespace PetShop.Web.ViewModels
{
    public class ProductOutputViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Qty { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public static ProductOutputViewModel FromApiModel(ProductViewModel model)
        {
            return new ProductOutputViewModel
            {
                Id = model.Id.ToString(),
                Name = model.Name,
                Price = model.Price,
                Qty = model.Qty,
                Description = model.Description,
                Image = model.Image
            };
        }
    }
}