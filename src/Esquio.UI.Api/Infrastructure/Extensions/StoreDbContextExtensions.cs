using Esquio.EntityFrameworkCore.Store.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.EntityFrameworkCore.Store
{
    public static class StoreDbContextExtensions
    {
        public static ValueTask<ProductEntity> GetProductBy(this StoreDbContext dbContext, int id)
        {
            return dbContext.Products.FindAsync(id);
        }

        public static async Task<FeatureEntity> GetFeatureOrThrow(
            this StoreDbContext dbContext,
            int featureId, 
            CancellationToken cancellationToken = default)
        {
            var flag = await dbContext
                .Features
                .Include(f => f.Toggles)
                .SingleOrDefaultAsync(f => f.Id == featureId, cancellationToken);

            if (flag == null)
            {
                throw new InvalidOperationException("Feature doesn't exist.");
            }

            return flag;
        }

        public static Task<FeatureTagEntity> GetFeatureTagBy(
            this StoreDbContext dbContext,
            int featureId,
            string tag,
            CancellationToken cancellationToken = default)
        {
            return dbContext
                .FeatureTagEntities
                .Include(ft => ft.TagEntity)
                .SingleOrDefaultAsync(ft => ft.FeatureEntityId == featureId && ft.TagEntity.Name == tag, cancellationToken);
        }

        public static async Task<FeatureTagEntity> GetFeatureTagOrThrow(
            this StoreDbContext dbContext,
            int featureId,
            string tag,
            CancellationToken cancellationToken = default)
        {
            var featureTag = await GetFeatureTagBy(dbContext, featureId, tag, cancellationToken);

            if (featureTag == null)
            {
                throw new InvalidOperationException($"There is no tagged feature with the tag {tag}");
            }

            return featureTag;
        }
    }
}
