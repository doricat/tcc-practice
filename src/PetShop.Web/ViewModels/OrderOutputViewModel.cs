using System;
using System.Collections.Generic;
using System.Linq;
using ViewModels.Shared.Order;

namespace PetShop.Web.ViewModels
{
    public class OrderOutputViewModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string TransactionId { get; set; }

        public string State { get; set; }

        public DateTime CreatedAt { get; set; }

        public IList<OrderItemOutputViewModel> Items { get; set; }

        public static OrderOutputViewModel FromApiModel(OrderViewModel model)
        {
            return new OrderOutputViewModel
            {
                Id = model.Id.ToString(),
                UserId = model.UserId.ToString(),
                TransactionId = model.TransactionId.ToString(),
                State = model.State,
                CreatedAt = model.CreatedAt,
                Items = model.Items.Select(OrderItemOutputViewModel.FromApiModel).ToList()
            };
        }
    }
}