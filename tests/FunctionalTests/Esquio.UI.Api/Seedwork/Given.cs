using Esquio.UI.Api.Infrastructure.Data.Entities;
using System.Threading.Tasks;

namespace FunctionalTests.Esquio.UI.Api.Seedwork
{
    public class Given
    {
        private readonly ServerFixture _serverFixture;

        public Given(ServerFixture serverFixture)
        {
            _serverFixture = serverFixture;
        }

        public async Task AddApiKey(params ApiKeyEntity[] apikeys)
        {
            await _serverFixture.ExecuteDbContextAsync(async db =>
            {
                await db.AddRangeAsync(apikeys);
                await db.SaveChangesAsync();
            });
        }

        public async Task AddProduct(params ProductEntity[] products)
        {
            await _serverFixture.ExecuteDbContextAsync(async db =>
            {
                await db.AddRangeAsync(products);
                await db.SaveChangesAsync();
            });
        }

        public async Task AddPermission(params PermissionEntity[] permissions)
        {
            await _serverFixture.ExecuteDbContextAsync(async db =>
            {
                await db.AddRangeAsync(permissions);
                await db.SaveChangesAsync();
            });
        }

        public async Task AddRing(params RingEntity[] rings)
        {
            await _serverFixture.ExecuteDbContextAsync(async db =>
            {
                await db.AddRangeAsync(rings);
            });
        }

        public async Task AddHistory(params HistoryEntity[] histories)
        {
            await _serverFixture.ExecuteDbContextAsync(async db =>
            {
                await db.AddRangeAsync(histories);
                await db.SaveChangesAsync();
            });
        }
    }
}
