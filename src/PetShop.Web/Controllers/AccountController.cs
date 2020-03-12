using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace PetShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IOptionsMonitor<OidcOptions> _optionsMonitor;

        public AccountController(IOptionsMonitor<OidcOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }

        public IActionResult Register()
        {
            return Redirect($"{_optionsMonitor.CurrentValue.Authority}/identity/account/register");
        }

        public IActionResult Manage()
        {
            return Redirect($"{_optionsMonitor.CurrentValue.Authority}/identity/account/manage");
        }
    }
}