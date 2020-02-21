using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Order.Api.Web.Controllers.ShoppingCar
{
    [ApiController]
    [Route("shopping_cars")]
    public class ShoppingCarsController: ControllerBase
    {
        public ShoppingCarsController(ILogger<ShoppingCarsController> logger)
        {
            Logger = logger;
        }

        public ILogger<ShoppingCarsController> Logger { get; }
    }
}