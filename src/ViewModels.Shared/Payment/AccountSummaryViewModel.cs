using System.Collections.Generic;

namespace ViewModels.Shared.Payment
{
    public class AccountSummaryViewModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public decimal AvailableBalance { get; set; }

        public IList<AccountBillViewModel> Items { get; set; }
    }
}