using ViewModels.Shared.Payment;

namespace PetShop.Web.ViewModels
{
    public class BillOutputViewModel
    {
        public string Id { get; set; }

        public string TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string State { get; set; }

        public static BillOutputViewModel FromApiModel(AccountBillViewModel model)
        {
            return new BillOutputViewModel
            {
                Id = model.Id.ToString(),
                TransactionId = model.TransactionId.ToString(),
                Amount = model.Amount,
                State = model.State
            };
        }
    }
}