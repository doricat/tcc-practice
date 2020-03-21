using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetShop.Web.ViewModels;
using ViewModels.Shared.Payment;

namespace PetShop.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("payment_accounts")]
    public class PaymentAccountsController : ControllerBase
    {
        public PaymentAccountsController(ILogger<PaymentAccountsController> logger,
            IHttpClientFactory clientFactory,
            IConfiguration configuration)
        {
            Logger = logger;
            ClientFactory = clientFactory;
            Configuration = configuration;
        }

        public ILogger<PaymentAccountsController> Logger { get; }

        public IHttpClientFactory ClientFactory { get; }

        public IConfiguration Configuration { get; }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = long.Parse(User.Claims.First(x => x.Type == "sub").Value);
            var client = ClientFactory.CreateClient();
            var accountResp = await client.GetAsync($"{Configuration["Payment"]}/accounts/{userId}");
            var content = await accountResp.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResult<AccountSummaryViewModel>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return Ok(new ApiResult<AccountSummaryOutputViewModel>(AccountSummaryOutputViewModel.FromApiModel(result.Value)));
        }
    }
}