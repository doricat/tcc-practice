using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ViewModels.Shared;

namespace Coordinator.Api.Web.Controllers
{
    [ApiController]
    [Route("coordinator")]
    public class CoordinatorController : ControllerBase
    {
        public CoordinatorController(ILogger<CoordinatorController> logger, IHttpClientFactory clientFactory)
        {
            Logger = logger;
            ClientFactory = clientFactory;
        }

        public ILogger<CoordinatorController> Logger { get; }

        public IHttpClientFactory ClientFactory { get; }

        [HttpPut("confirm")]
        public Task<IActionResult> Confirm(CoordinatorViewModel model)
        {
            var client = ClientFactory.CreateClient();
            var tasks = new Task[model.Links.Count];
            var i = 0;
            foreach (var link in model.Links)
            {
                Logger.LogInformation("Confirm {uri}.", link.Uri);
                tasks[i++] = client.PutAsync(link.Uri, null, HttpContext.RequestAborted);
            }

            Task.WaitAll(tasks, HttpContext.RequestAborted);

            var b = tasks.Cast<Task<HttpResponseMessage>>().Any(x => !x.Result.IsSuccessStatusCode);
            return b ? Task.FromResult<IActionResult>(NotFound()) : Task.FromResult<IActionResult>(NoContent());

            // TODO 强制取消机制
        }

        [HttpPut("cancel")]
        public Task<IActionResult> Cancel(CoordinatorViewModel model)
        {
            var client = ClientFactory.CreateClient();
            var tasks = new Task[model.Links.Count];
            var i = 0;
            foreach (var link in model.Links)
            {
                Logger.LogInformation("Cancel {uri}.", link.Uri);
                tasks[i++] = client.DeleteAsync(link.Uri, HttpContext.RequestAborted);
            }

            Task.WaitAll(tasks, HttpContext.RequestAborted);

            var b = tasks.Cast<Task<HttpResponseMessage>>().Any(x => !x.Result.IsSuccessStatusCode);
            return b ? Task.FromResult<IActionResult>(NotFound()) : Task.FromResult<IActionResult>(NoContent());
        }
    }
}
