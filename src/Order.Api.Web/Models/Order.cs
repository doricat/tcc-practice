using System;
using System.Collections.Generic;
using Domain.Shared;

namespace Order.Api.Web.Models
{
    public class Order : Entity
    {
        public long UserId { get; set; }

        public long TransactionId { get; set; }

        public TccState State { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime Expires { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; }
    }
}