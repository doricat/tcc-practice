using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PetShop.Web.Models;

namespace PetShop.Web.Controllers
{
    public class HomeController : Controller
    {
        //public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}