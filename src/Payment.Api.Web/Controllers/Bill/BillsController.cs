using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Payment.Api.Web.Controllers.Bill
{
    [ApiController]
    [Route("bills")]
    public class BillsController : ControllerBase
    {
        public BillsController(ILogger<BillsController> logger)
        {
            Logger = logger;
        }

        public ILogger<BillsController> Logger { get; }
    }
}
