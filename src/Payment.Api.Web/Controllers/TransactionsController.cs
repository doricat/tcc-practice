using System;
using System.Threading.Tasks;
using ApiModels;
using Dapper;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Payment.Api.Web.Models;
using ViewModels.Shared.Payment;
using Web.Shared;

namespace Payment.Api.Web.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class TransactionsController : ControllerBase
    {
        public TransactionsController(ILogger<TransactionsController> logger,
            PaymentDbContext dbContext,
            CancellationTaskRabbitMqMessageSender messageSender,
            IdentityGenerator generator, 
            IConfiguration configuration)
        {
            Logger = logger;
            DbContext = dbContext;
            MessageSender = messageSender;
            Generator = generator;
            Configuration = configuration;
        }

        public ILogger<TransactionsController> Logger { get; }

        public PaymentDbContext DbContext { get; }

        public CancellationTaskRabbitMqMessageSender MessageSender { get; }

        public IdentityGenerator Generator { get; }

        public IConfiguration Configuration { get; }

        [HttpPost]
        public async Task<IActionResult> Post(BillCreationInputModel model)
        {
            var now = DateTime.Now;
            var expires = now.AddMilliseconds(Configuration.GetValue<int>("TransactionTimeout"));
            var id = Generator.Generate();
            var result = await DbContext.Database.GetDbConnection()
                .QueryFirstOrDefaultAsync<BillCreatingResult>(@"select create_bill(@id, @uid, @amount, @tid, @now, @expires);", new
                {
                    id,
                    uid = model.UserId,
                    amount = model.Amount,
                    tid = model.TransactionId,
                    now,
                    expires
                });

            if (result == BillCreatingResult.Ok)
            {
                var uri = $"{Request.Scheme}://{Request.Host}/transactions/{model.TransactionId}";

                await MessageSender.SendAsync(new CancellationItem
                {
                    Uri = uri
                });

                Logger.LogInformation("账单创建成功: {id}, {uri}", model.TransactionId, uri);

                return Created(uri, new TransactionObjectCreatedOutputModel<long>(model.TransactionId, uri, expires));
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
                .QueryFirstOrDefaultAsync<bool>(@"select confirm_bill(@tid, @now);", new
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
                .QueryFirstOrDefaultAsync<bool>(@"select cancel_bill(@tid, @now);", new
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