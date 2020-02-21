using Microsoft.AspNetCore.SignalR;

namespace TransactionMonitor.Web.Hubs.Transaction
{
    public class TransactionHub : Hub<ITransactionsClient>
    {
        
    }
}