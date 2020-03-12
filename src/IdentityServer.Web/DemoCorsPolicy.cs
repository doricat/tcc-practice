using System.Threading.Tasks;
using IdentityServer4.Services;

namespace IdentityServer.Web
{
    public class DemoCorsPolicy : ICorsPolicyService
    {
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            return Task.FromResult(true);
        }
    }
}