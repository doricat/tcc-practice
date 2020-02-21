using System.Threading.Tasks;

namespace TransactionMonitor.Web.Hubs.Transaction
{
    public interface ITransactionsClient
    {
        Task ReceiveMessageAsync(TransactionViewModel message);
    }
}