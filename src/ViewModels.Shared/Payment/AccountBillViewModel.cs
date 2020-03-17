namespace ViewModels.Shared.Payment
{
    public class AccountBillViewModel
    {
        public long Id { get; set; }

        public long TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string State { get; set; }
    }
}