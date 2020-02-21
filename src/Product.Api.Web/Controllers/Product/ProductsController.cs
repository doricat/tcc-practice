using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Product.Api.Web.Controllers.Product
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        public ProductsController(ILogger<ProductsController> logger)
        {
            Logger = logger;
        }

        public ILogger<ProductsController> Logger { get; }
    }
}