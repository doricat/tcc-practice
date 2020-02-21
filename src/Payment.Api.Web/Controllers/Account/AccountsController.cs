using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Payment.Api.Web.Controllers.Account
{
    [ApiController]
    [Route("accounts")]
    public class AccountsController : ControllerBase
    {
        public AccountsController(ILogger<AccountsController> logger)
        {
            Logger = logger;
        }

        public ILogger<AccountsController> Logger { get; }
    }
}