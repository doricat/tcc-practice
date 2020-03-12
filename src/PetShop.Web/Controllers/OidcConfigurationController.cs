using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace PetShop.Web.Controllers
{
    public class OidcConfigurationController : Controller
    {
        private readonly IOptionsMonitor<OidcOptions> _optionsMonitor;

        public OidcConfigurationController(IOptionsMonitor<OidcOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }

        [HttpGet("_configuration")]
        public IActionResult GetClientRequestParameters()
        {
            var parameters = _optionsMonitor.CurrentValue;
            return Ok(parameters);
        }
    }
}