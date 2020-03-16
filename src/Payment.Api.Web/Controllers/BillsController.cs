using System;
using System.Threading.Tasks;
using ApiModels;
using Dapper;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payment.Api.Web.Models;
using ViewModels.Shared.Payment;
using Web.Shared;

namespace Payment.Api.Web.Controllers
{
    [ApiController]
    [Route("bills")]
    public class BillsController : ControllerBase
    {
        public BillsController(ILogger<BillsController> logger,
            PaymentDbContext dbContext,
            CancellationTaskRabbitMqMessageSender messageSender,
            IdentityGenerator generator)
        {
            Logger = logger;
            DbContext = dbContext;
            MessageSender = messageSender;
            Generator = generator;
        }

        public ILogger<BillsController> Logger { get; }

        public PaymentDbContext DbContext { get; }

        public CancellationTaskRabbitMqMessageSender MessageSender { get; }

        public IdentityGenerator Generator { get; }

        [HttpPost]
        public async Task<IActionResult> Post(BillCreationInputModel model)
        {
            var now = DateTime.Now;
            var expires = now.AddMilliseconds(model.Timeout);
            var id = Generator.Generate();
            var result = await DbContext.Database.GetDbConnection()
                .QueryFirstOrDefaultAsync<BillCreatingResult>(@"select create_bill(@id, @uid, @amount, @tid, @now, @expires);", new
                {
                    id,
                    uid = model.UserId,
                    amount = model.Amount,
                    bid = model.TransactionId,
                    now,
                    expires
                });

            if (result == BillCreatingResult.Ok)
            {
                var uri = $"{Request.Scheme}://{Request.Host}/bills/{model.TransactionId}";

                await MessageSender.SendAsync(new CancellationItem
                {
                    Uri = uri
                });

                Logger.LogInformation("账单创建成功: {id}, {uri}", model.TransactionId, uri);

                return Created(uri, new ObjectCreatedOutputModel<long>(model.TransactionId, uri));
            }

            if (result == BillCreatingResult.InsufficientBalance)
            {
                return BadRequest(new ApiErrorResult<ApiError>(new ApiError("InsufficientBalance", "InsufficientBalance")));
            }

            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id)
        {
            var result = await DbContext.Database.GetDbConnection()
                .QueryFirstOrDefaultAsync<bool>(@"select confirm_bill(@bid, @now);", new
                {
                    bid = id,
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
                .QueryFirstOrDefaultAsync<bool>(@"select cancel_bill(@bid, @now);", new
                {
                    bid = id,
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