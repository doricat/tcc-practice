using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModels;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ViewModels.Shared;
using ViewModels.Shared.Order;
using ViewModels.Shared.Payment;
using ViewModels.Shared.Product;

namespace PetShop.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        public OrdersController(ILogger<OrdersController> logger,
            IHttpClientFactory clientFactory,
            IdentityGenerator generator,
            IConfiguration configuration)
        {
            Logger = logger;
            ClientFactory = clientFactory;
            Generator = generator;
            Configuration = configuration;
        }

        public ILogger<OrdersController> Logger { get; }

        public IHttpClientFactory ClientFactory { get; }

        public IdentityGenerator Generator { get; }

        public IConfiguration Configuration { get; }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = long.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var client = ClientFactory.CreateClient();
            var orderResp = await client.GetAsync($"{Configuration["Order"]}/orders/{userId}");
            var content = await orderResp.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResult<IList<OrderViewModel>>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderCreationInputModel2 model)
        {
            var userId = long.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var client = ClientFactory.CreateClient();
            var transactionId = Generator.Generate();

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var productInfoResp = await client.GetAsync($"{Configuration["Product"]}/products/{model.ProductId}");
            var productInfoContent = await productInfoResp.Content.ReadAsStringAsync();
            if (!productInfoResp.IsSuccessStatusCode)
            {
                Logger.LogError("获取产品信息失败, {message}", productInfoContent);
                return StatusCode(500, new ApiErrorResult<ApiError>(new ApiError("ServerError", "获取产品信息失败")));
            }

            var productInfo = JsonSerializer.Deserialize<ProductViewModel>(productInfoContent, jsonSerializerOptions);

            var saleTask = client.PostAsync($"{Configuration["Product"]}/transactions",
                new StringContent(JsonSerializer.Serialize(new ProductReserveInputModel
                {
                    UserId = userId,
                    TransactionId = transactionId,
                    ProductId = model.ProductId,
                    Qty = 1
                }), Encoding.UTF8, "application/json"));

            var orderTask = client.PostAsync($"{Configuration["Order"]}/transactions",
                new StringContent(JsonSerializer.Serialize(new OrderCreationInputModel
                {
                    UserId = userId,
                    TransactionId = transactionId,
                    Items = new List<OrderItemCreationInputModel>
                    {
                        new OrderItemCreationInputModel
                        {
                            ProductId = model.ProductId,
                            Qty = 1,
                            Price = productInfo.Price,
                            Image = productInfo.Image,
                            Name = productInfo.Name
                        }
                    }
                }), Encoding.UTF8, "application/json"));

            var billTask = client.PostAsync($"{Configuration["Payment"]}/transactions",
                new StringContent(JsonSerializer.Serialize(new BillCreationInputModel
                {
                    UserId = userId,
                    TransactionId = transactionId,
                    Amount = -productInfo.Price
                }), Encoding.UTF8, "application/json"));

            Task.WaitAll(saleTask, billTask);

            var saleResult = await saleTask.Result.Content.ReadAsStringAsync();
            var orderResult = await orderTask.Result.Content.ReadAsStringAsync();
            var billResult = await billTask.Result.Content.ReadAsStringAsync();

            Logger.LogInformation("产品系统返回结果: {code} {json}", saleTask.Result.StatusCode, saleResult);
            Logger.LogInformation("订单系统返回结果: {code} {json}", orderTask.Result.StatusCode, orderResult);
            Logger.LogInformation("账单系统返回结果: {code} {json}", billTask.Result.StatusCode, billResult);

            if (!saleTask.Result.IsSuccessStatusCode)
            {
                var apiResult = JsonSerializer.Deserialize<ApiErrorResult<ApiError>>(saleResult, jsonSerializerOptions);
                if (apiResult.Error.Code.Equals("OutOfStock", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(apiResult); // 等待系统自动撤销账单
                }

                throw new NotImplementedException();
            }

            if (!billTask.Result.IsSuccessStatusCode)
            {
                var apiResult = JsonSerializer.Deserialize<ApiErrorResult<ApiError>>(billResult, jsonSerializerOptions);
                if (apiResult.Error.Code.Equals("InsufficientBalance", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(apiResult);
                }

                throw new NotImplementedException();
            }

            // 演示需求
            var saleInfo = JsonSerializer.Deserialize<TransactionObjectCreatedOutputModel<long>>(saleResult, jsonSerializerOptions);
            var orderInfo = JsonSerializer.Deserialize<TransactionObjectCreatedOutputModel<long>>(orderResult, jsonSerializerOptions);
            var billInfo = JsonSerializer.Deserialize<TransactionObjectCreatedOutputModel<long>>(billResult, jsonSerializerOptions);
            var links = new List<Link>
            {
                new Link
                {
                    Uri = saleInfo.Location,
                    Expires = saleInfo.Expires
                },
                new Link
                {
                    Uri = billInfo.Location,
                    Expires = billInfo.Expires
                },
                new Link
                {
                    Uri = orderInfo.Location,
                    Expires = orderInfo.Expires
                }
            };

            var resp = await client.PutAsync($"{Configuration["Coordinator"]}/coordinator/confirm",
                new StringContent(JsonSerializer.Serialize(new CoordinatorViewModel
                {
                    Links = links
                }), Encoding.UTF8, "application/json"));

            Logger.LogInformation("协调器返回结果: {code}", resp.StatusCode);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ManualConfirmOrderInputModel model)
        {
            var client = ClientFactory.CreateClient();

            var resp = await client.PutAsync($"{Configuration["Coordinator"]}/coordinator/confirm",
                new StringContent(JsonSerializer.Serialize(new CoordinatorViewModel
                {
                    Links = model.Links
                }), Encoding.UTF8, "application/json"));

            Logger.LogInformation("协调器返回结果: {code}", resp.StatusCode);

            return NoContent();
        }
    }

    public class OrderCreationInputModel2
    {
        public long ProductId { get; set; }
    }

    public class ManualConfirmOrderInputModel
    {
        public IList<Link> Links { get; set; }
    }
}