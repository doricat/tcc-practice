using System.Collections.Generic;
using Domain.Shared;

namespace Product.Api.Web.Models
{
    public class Product : Entity
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Qty { get; set; }

        public string Description { get; set; }

        public virtual ICollection<SaleLog> SaleLogs { get; set; }
    }
}