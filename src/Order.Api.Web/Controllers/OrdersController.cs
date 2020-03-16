using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Order.Api.Web.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        public OrdersController(ILogger<OrdersController> logger)
        {
            Logger = logger;
        }

        public ILogger<OrdersController> Logger { get; }


    }
}