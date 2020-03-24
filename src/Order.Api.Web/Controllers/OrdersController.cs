using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Api.Web.Models;
using ViewModels.Shared.Order;

namespace Order.Api.Web.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        public OrdersController(ILogger<OrdersController> logger, OrderDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        public ILogger<OrdersController> Logger { get; }

        public OrderDbContext DbContext { get; }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get([FromRoute] long userId)
        {
            var orders = await DbContext.Orders.Include(x => x.Items).Where(x => x.UserId == userId).ToListAsync();
            var result = orders.Select(x => new OrderViewModel
            {
                Id = x.Id,
                UserId = x.UserId,
                CreatedAt = x.CreatedAt,
                State = x.State.ToString(),
                TransactionId = x.TransactionId,
                Items = x.Items.Select(y => new OrderItemViewModel
                {
                    Id = y.Id,
                    Qty = y.Qty,
                    ProductId = y.ProductId,
                    Price = y.Price,
                    Name = y.Name,
                    Image = y.Image
                }).ToList()
            }).ToList();

            return Ok(new ApiResult<IList<OrderViewModel>>(result));
        }
    }
}