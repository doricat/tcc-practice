using System.Collections.Generic;
using System.Linq;
using ViewModels.Shared.Payment;

namespace PetShop.Web.ViewModels
{
    public class AccountSummaryOutputViewModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public decimal AvailableBalance { get; set; }

        public IList<BillOutputViewModel> Items { get; set; }

        public static AccountSummaryOutputViewModel FromApiModel(AccountSummaryViewModel model)
        {
            return new AccountSummaryOutputViewModel
            {
                Id = model.Id.ToString(),
                UserId = model.UserId.ToString(),
                AvailableBalance = model.AvailableBalance,
                Items = model.Items.Select(BillOutputViewModel.FromApiModel).ToList()
            };
        }
    }
}