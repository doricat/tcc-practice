using System.Threading.Tasks;

namespace TransactionMonitor.Web.Hubs.Transaction
{
    public interface ITransactionsClient
    {
        Task ReceiveMessage(TransactionViewModel message);
    }
}