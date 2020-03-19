using System;
using System.Linq;
using System.Threading.Tasks;
using ApiModels;
using Dapper;
using Domain.Shared;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Order.Api.Web.Models;
using ViewModels.Shared.Order;
using Web.Shared;

namespace Order.Api.Web.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class TransactionsController : ControllerBase
    {
        public TransactionsController(ILogger<TransactionsController> logger,
            IdentityGenerator generator,
            OrderDbContext dbContext,
            CancellationTaskRabbitMqMessageSender messageSender, 
            IConfiguration configuration)
        {
            Logger = logger;
            Generator = generator;
            DbContext = dbContext;
            MessageSender = messageSender;
            Configuration = configuration;
        }

        public ILogger<TransactionsController> Logger { get; }

        public IdentityGenerator Generator { get; }

        public OrderDbContext DbContext { get; }

        public CancellationTaskRabbitMqMessageSender MessageSender { get; }

        public IConfiguration Configuration { get; }

        [HttpPost]
        public async Task<IActionResult> Post(OrderCreationInputModel model)
        {
            var now = DateTime.Now;
            var expires = now.AddMilliseconds(Configuration.GetValue<int>("TransactionTimeout"));
            var id = Generator.Generate();

            var order = new Models.Order
            {
                Id = id,
                TransactionId = model.TransactionId,
                UserId = model.UserId,
                State = TccState.Pending,
                CreatedAt = now,
                Expires = expires,
                UpdatedAt = now,
                Items = model.Items.Select(x => new OrderItem
                {
                    Id = Generator.Generate(),
                    OrderId = id,
                    ProductId = x.ProductId,
                    Qty = x.Qty,
                    Price = x.Price,
                    Name = x.Name,
                    Image = x.Image
                }).ToList()
            };

            DbContext.Orders.Add(order);
            await DbContext.SaveChangesAsync();

            var uri = $"{Request.Scheme}://{Request.Host}/transactions/{model.TransactionId}";

            await MessageSender.SendAsync(new CancellationItem
            {
                Uri = uri
            });

            Logger.LogInformation("订单创建成功: {id}, {uri}", model.TransactionId, uri);

            return Created(uri, new TransactionObjectCreatedOutputModel<long>(model.TransactionId, uri, expires));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id)
        {
            var result = await DbContext.Database.GetDbConnection()
                .QueryFirstOrDefaultAsync<bool>(@"select confirm_order(@tid, @now);", new
                {
                    tid = id,
                    now = DateTime.Now
                });

            if (result)
            {
                Logger.LogInformation("确认成功: {id}", id);
                return NoContent();
            }

            Logger.LogInformation("已确认或已撤销: {id}", id);
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await DbContext.Database.GetDbConnection()
                .QueryFirstOrDefaultAsync<bool>(@"select cancel_order(@tid, @now);", new
                {
                    tid = id,
                    now = DateTime.Now
                });

            if (result)
            {
                Logger.LogInformation("取消成功: {id}", id);
                return NoContent();
            }

            Logger.LogInformation("已确认或已撤销: {id}", id);
            return NotFound();
        }
    }
}