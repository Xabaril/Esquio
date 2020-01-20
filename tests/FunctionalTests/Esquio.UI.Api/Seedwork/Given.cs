using Esquio.EntityFrameworkCore.Store.Entities;
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
                db.AddRange(apikeys);

                await db.SaveChangesAsync();
            });
        }

        public async Task AddProduct(params ProductEntity[] products)
        {
            await _serverFixture.ExecuteDbContextAsync(async db =>
            {
                db.AddRange(products);

                await db.SaveChangesAsync();
            });
        }

        public async Task AddPermission(params PermissionEntity[] permissions)
        {
            await _serverFixture.ExecuteDbContextAsync(async db =>
            {
                db.AddRange(permissions);

                await db.SaveChangesAsync();
            });
        }

        public async Task AddRing(params RingEntity[] rings)
        {
            await _serverFixture.ExecuteDbContextAsync(async db =>
            {
                db.AddRange(rings);

                await db.SaveChangesAsync();
            });
        }
    }
}
