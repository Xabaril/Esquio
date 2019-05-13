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

        public async Task<Product> FindProductAsync(string name)
        {
            var productEntity = await GetProductEntityOrThrow(name);
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

            var productEntity = await GetProductEntityOrThrow(product.Name);
            productEntity.CopyFrom(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            Ensure.Argument.NotNull(product, nameof(product));

            var productEntity = await GetProductEntityOrThrow(product.Name);
            _dbContext.Remove(productEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddFeatureAsync(string productName, Feature feature)
        {
            Ensure.Argument.NotNullOrEmpty(productName);
            Ensure.Argument.NotNull(feature);

            var productEntity = await GetProductEntityOrThrow(productName);
            productEntity.Features.Add(feature.To());

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateFeatureAsync(string product, Feature feature)
        {
            Ensure.Argument.NotNullOrEmpty(product);
            Ensure.Argument.NotNull(feature);

            await DeleteFeatureAsync(product, feature);
            await AddFeatureAsync(product, feature);
        }

        public async Task DeleteFeatureAsync(string product, Feature feature)
        {
            Ensure.Argument.NotNullOrEmpty(product);
            Ensure.Argument.NotNull(feature);

            var featureEntity = await GetFeatureEntityOrThrow(feature.Name, product);
            _dbContext.Remove(featureEntity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Feature> FindFeatureAsync(string featureName, string productName)
        {
            var featureEntity = await GetFeatureEntityOrThrow(featureName, productName);

            if (featureEntity != null)
            {
                return featureEntity.To();
            }

            return null;
        }

        private async Task<FeatureEntity> GetFeatureEntityOrThrow(string featureName, string productName)
        {
            var featureEntity = await _dbContext
                .Products
                .Where(a => a.Name == productName)
                .Join(_dbContext.Features,
                    a => a.Id,
                    f => f.ProductId,
                    (a, f) => f)
                .Include(f => f.Toggles)
                .ThenInclude(t => t.Parameters)
                .SingleOrDefaultAsync(f => f.Name == featureName);

            if (featureEntity == null)
            {
                Log.FeatureNotExist(_logger, featureName, productName);
                throw new EsquioException($"The feature with name ${featureName} for product ${productName} does not exist.");
            }

            return featureEntity;
        }

        private async Task<ProductEntity> GetProductEntityOrThrow(string productName)
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
