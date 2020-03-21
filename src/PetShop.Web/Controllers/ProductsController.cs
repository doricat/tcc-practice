using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetShop.Web.ViewModels;
using ViewModels.Shared.Product;

namespace PetShop.Web.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        public ProductsController(ILogger<ProductsController> logger,
            IHttpClientFactory clientFactory,
            IConfiguration configuration)
        {
            Logger = logger;
            ClientFactory = clientFactory;
            Configuration = configuration;
        }

        public ILogger<ProductsController> Logger { get; }

        public IHttpClientFactory ClientFactory { get; }

        public IConfiguration Configuration { get; }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var client = ClientFactory.CreateClient();
            var productResp = await client.GetAsync($"{Configuration["Product"]}/products/{id}");
            if (productResp.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            var content = await productResp.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResult<ProductViewModel>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return Ok(new ApiResult<ProductOutputViewModel>(ProductOutputViewModel.FromApiModel(result.Value)));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = ClientFactory.CreateClient();
            var productResp = await client.GetAsync($"{Configuration["Product"]}/products");
            var content = await productResp.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResult<IList<ProductViewModel>>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return Ok(new ApiResult<IList<ProductOutputViewModel>>(result.Value.Select(ProductOutputViewModel.FromApiModel).ToList()));
        }
    }
}