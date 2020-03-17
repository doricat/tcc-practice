using System;
using System.Threading.Tasks;
using ApiModels;
using Dapper;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Product.Api.Web.Models;
using ViewModels.Shared.Product;
using Web.Shared;

namespace Product.Api.Web.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class TransactionsController : ControllerBase
    {
        public TransactionsController(ILogger<TransactionsController> logger,
            ProductDbContext dbContext,
            CancellationTaskRabbitMqMessageSender messageSender,
            IdentityGenerator generator)
        {
            Logger = logger;
            DbContext = dbContext;
            MessageSender = messageSender;
            Generator = generator;
        }

        public ILogger<TransactionsController> Logger { get; }

        public ProductDbContext DbContext { get; }

        public CancellationTaskRabbitMqMessageSender MessageSender { get; }

        public IdentityGenerator Generator { get; }

        [HttpPost]
        public async Task<IActionResult> Post(ProductReserveInputModel model)
        {
            var now = DateTime.Now;
            var expires = now.AddMilliseconds(model.Timeout);
            var id = Generator.Generate();
            var result = await DbContext
                .Database
                .GetDbConnection()
                .QueryFirstOrDefaultAsync<SaleLogCreatingResult>(@"select create_log(@id, @uid, @tid, @pid, @qty, @now, @expires);",
                    new
                    {
                        id,
                        uid = model.UserId,
                        tid = model.TransactionId,
                        pid = model.ProductId,
                        qty = model.Qty,
                        now,
                        expires
                    });

            if (result == SaleLogCreatingResult.Ok)
            {
                var uri = $"{Request.Scheme}://{Request.Host}/transactions/{model.TransactionId}";

                await MessageSender.SendAsync(new CancellationItem
                {
                    Uri = uri
                });

                Logger.LogInformation("商品保留成功: {id}, {uri}", model.TransactionId, uri);

                return Created(uri, new TransactionObjectCreatedOutputModel<long>(model.TransactionId, uri, expires));
            }

            if (result == SaleLogCreatingResult.OutOfStock)
            {
                return BadRequest(new ApiErrorResult<ApiError>(new ApiError("OutOfStock", "OutOfStock")));
            }

            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id)
        {
            var result = await DbContext
                .Database
                .GetDbConnection()
                .QueryFirstOrDefaultAsync<bool>(@"select confirm_log(@tid, @now);",
                    new
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
            var result = await DbContext
                .Database
                .GetDbConnection()
                .QueryFirstOrDefaultAsync<bool>(@"select cancel_log(@tid, @now);",
                    new
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