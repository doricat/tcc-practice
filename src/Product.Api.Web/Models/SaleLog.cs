using Domain.Shared;

namespace Product.Api.Web.Models
{
    public class SaleLog : Entity
    {
        public long UserId { get; set; }

        public long TransactionId { get; set; }

        public long ProductId { get; set; }

        public int Qty { get; set; }

        public TccState State { get; set; }

        public virtual Product Product { get; set; }
    }
}