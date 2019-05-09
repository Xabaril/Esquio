using Esquio.Abstractions;
using Esquio.EntityFrameworkCore.Store.Diagnostics;
using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.EntityFrameworkCore.Store
{
    internal class EntityFrameworkCoreFeaturesStore : IFeatureStore
    {
        private readonly ILogger<EntityFrameworkCoreFeaturesStore> _logger;
        private readonly StoreDbContext _dbContext;

        public EntityFrameworkCoreFeaturesStore(ILogger<EntityFrameworkCoreFeaturesStore> logger, StoreDbContext dbContext)
        {
            Ensure.Argument.NotNull(logger, nameof(logger));
            Ensure.Argument.NotNull(dbContext, nameof(dbContext));

            _logger = logger;
            _dbContext = dbContext;
        }

        public bool IsReadOnly => false;

        public async Task AddFeatureAsync(string productName, Feature feature)
        {
            Ensure.Argument.NotNullOrEmpty(productName);
            Ensure.Argument.NotNull(feature);

            var productEntity = await GetProductOrThrow(productName);
            productEntity.Features.Add(feature.To());

            await _dbContext.SaveChangesAsync();
        }
        public async Task<Product> FindProductAsync(string name)
        {
            var productEntity = await GetProductOrThrow(name);
            return productEntity.To();
        }

        public async Task AddProductAsync(Product product)
        {
            Ensure.Argument.NotNull(product, nameof(product));

            await _dbContext.Products.AddAsync(product.To());
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            Ensure.Argument.NotNull(product, nameof(product));

            var productEntity = await GetProductOrThrow(product.Name);
            productEntity.CopyFrom(product);
            await _dbContext.SaveChangesAsync();
        }


        public async Task DeleteProductAsync(Product product)
        {
            Ensure.Argument.NotNull(product, nameof(product));

            var productEntity = await GetProductOrThrow(product.Name);
            _dbContext.Remove(productEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Feature> FindFeatureAsync(string featureName, string productName)
        {
            FeatureEntity feature = await FindFeatureEntityAsync(featureName, productName);

            if (feature != null)
            {
                return feature.To();
            }

            return null;
        }

        private async Task<FeatureEntity> FindFeatureEntityAsync(string featureName, string productName)
        {
            return await _dbContext
                .Products
                .Where(a => a.Name == productName)
                .Join(_dbContext.Features,
                    a => a.Id,
                    f => f.ProductId,
                    (a, f) => f)
                .Include(f => f.Toggles)
                .ThenInclude(t => t.Parameters)
                .SingleOrDefaultAsync(f => f.Name == featureName);
        }

        private async Task<ProductEntity> GetProductOrThrow(string productName)
        {
            var productEntity = await _dbContext.Products.SingleOrDefaultAsync(p => p.Name == productName);

            if (productEntity == null)
            {
                Log.ProductNotExist(_logger, productName);
                throw new EsquioException($"The product with name {productName} not exists.");
            }

            return productEntity;
        }

    }
}
