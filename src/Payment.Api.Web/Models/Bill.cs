using System;
using Domain.Shared;

namespace Payment.Api.Web.Models
{
    public class Bill : Entity
    {
        public long AccountId { get; set; }

        public decimal Amount { get; set; }

        public TccState State { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual Account Account { get; set; }
    }
}