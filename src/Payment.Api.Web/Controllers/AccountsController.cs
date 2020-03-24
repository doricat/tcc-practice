using System.Linq;
using System.Threading.Tasks;
using ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payment.Api.Web.Models;
using ViewModels.Shared.Payment;

namespace Payment.Api.Web.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountsController : ControllerBase
    {
        public AccountsController(ILogger<AccountsController> logger,
            PaymentDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        public ILogger<AccountsController> Logger { get; }

        public PaymentDbContext DbContext { get; }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var account = await DbContext.Accounts.Include(x => x.Bills).FirstOrDefaultAsync(x => x.UserId == id);

            if (account == null)
            {
                return NotFound(new ApiErrorResult<ApiError>(new ApiError("AccountNotFound", "AccountNotFound")));
            }

            return Ok(new ApiResult<AccountSummaryViewModel>(new AccountSummaryViewModel
            {
                Id = account.Id,
                UserId = account.UserId,
                AvailableBalance = account.Balance,
                Items = account.Bills.Select(x => new AccountBillViewModel
                {
                    Id = x.Id,
                    TransactionId = x.TransactionId,
                    State = x.State.ToString(),
                    Amount = x.Amount,
                    CreatedAt = x.CreatedAt
                }).ToList()
            }));
        }
    }
}