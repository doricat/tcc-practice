using System.Collections.Generic;
using Domain.Shared;

namespace Payment.Api.Web.Models
{
    public class Account : Entity
    {
        public long UserId { get; set; }

        public decimal Balance { get; set; }

        public virtual ICollection<Bill> Bills { get; set; }
    }
}