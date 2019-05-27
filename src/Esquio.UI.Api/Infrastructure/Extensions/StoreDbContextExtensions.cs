using Esquio.EntityFrameworkCore.Store.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Esquio.EntityFrameworkCore.Store
{
    public static class StoreDbContextExtensions
    {
        public static ValueTask<ProductEntity> GetProductBy(this StoreDbContext dbContext, int id)
        {
            return dbContext.Products.FindAsync(id);
        }

        public static Task<FeatureEntity> GetFlagBy(this StoreDbContext dbContext, int productId, int flagId)
        {
            return dbContext
                .Features
                .Include(f => f.Toggles)
                .SingleOrDefaultAsync(f => f.Id == flagId && f.ProductEntityId == productId);
        }
    }
}
