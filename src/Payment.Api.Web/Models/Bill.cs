using System;
using Domain.Shared;

namespace Payment.Api.Web.Models
{
    public class Bill : Entity
    {
        public long AccountId { get; set; }

        public decimal Amount { get; set; }

        public long TransactionId { get; set; }

        public TccState State { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime Expires { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual Account Account { get; set; }
    }
}