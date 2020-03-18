namespace ViewModels.Shared.Payment
{
    public class BillCreationInputModel
    {
        public long UserId { get; set; }

        public long TransactionId { get; set; }

        public decimal Amount { get; set; }
    }
}