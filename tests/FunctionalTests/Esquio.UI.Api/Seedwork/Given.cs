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
        public async Task AddProduct(params ProductEntity[] products)
        {
            await _serverFixture.ExecuteDbContextAsync(async db =>
            {
                db.AddRange(products);

                await db.SaveChangesAsync();
            });
        }
    }
}
