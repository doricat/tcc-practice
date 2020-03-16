using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Product.Api.Web.Models;
using ViewModels.Shared.Product;

namespace Product.Api.Web.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        public ProductsController(ILogger<ProductsController> logger,
            ProductDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        public ILogger<ProductsController> Logger { get; }

        public ProductDbContext DbContext { get; }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await DbContext.Products.ToListAsync(HttpContext.RequestAborted);
            return Ok(new ApiResult<IList<ProductViewModel>>(products.Select(ToViewModel).ToList()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var product = await DbContext.Products.FindAsync(id, HttpContext.RequestAborted);
            return Ok(new ApiResult<ProductViewModel>(ToViewModel(product)));
        }

        private static ProductViewModel ToViewModel(Models.Product entity)
        {
            return new ProductViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                Qty = entity.Qty,
                Description = entity.Description
            };
        }
    }
}