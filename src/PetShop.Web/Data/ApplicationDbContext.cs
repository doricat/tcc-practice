using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetShop.Web.Models;

namespace PetShop.Web.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext2<User, Role, long>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }
}