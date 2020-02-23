using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace TransactionMonitor.Web.Hubs.Transaction
{
    public class TransactionHub : Hub<ITransactionsClient>
    {
        public TransactionHub(ILogger<TransactionHub> logger)
        {
            Logger = logger;
        }

        public ILogger<TransactionHub> Logger { get; }

        public override Task OnConnectedAsync()
        {
            Logger.LogInformation("客户端 {client} 已连接。", Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Logger.LogInformation("客户端 {client} 已断开。", Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}