using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Esquio.UI.Api.Infrastructure.Data.DbContexts
{
    public class NpgSqlContext : StoreDbContext
    {
        public NpgSqlContext(DbContextOptions dbContextOptions, IOptions<StoreOptions> storeOptions) : base(dbContextOptions, storeOptions)
        {
        }
    }
}
