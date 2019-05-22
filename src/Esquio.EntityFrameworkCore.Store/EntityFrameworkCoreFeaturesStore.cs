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
    internal class EntityFrameworkCoreFeaturesStore
        : IRuntimeFeatureStore
    {
        const string DEFAULT_PRODUCT_NAME = "default";

        private readonly ILogger<EntityFrameworkCoreFeaturesStore> _logger;
        private readonly StoreDbContext _dbContext;

        public EntityFrameworkCoreFeaturesStore(ILogger<EntityFrameworkCoreFeaturesStore> logger, StoreDbContext dbContext)
        {
            Ensure.Argument.NotNull(logger, nameof(logger));
            Ensure.Argument.NotNull(dbContext, nameof(dbContext));

            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Feature> FindFeatureAsync(string featureName, string productName = null)
        {
            Log.FindFeature(_logger, featureName, productName);

            productName = productName ?? DEFAULT_PRODUCT_NAME;

            var featureEntity = await _dbContext.Features
                .Where(f => f.Name == featureName && f.ProductEntity.Name == productName)
                .Include(f => f.Toggles).ThenInclude(t => t.Parameters)
                .SingleOrDefaultAsync();

            if (featureEntity != null)
            {
                return featureEntity.To();
            }
            else
            {
                Log.FeatureNotExist(_logger, featureName, productName);
                return null;
            }
        }
    }
}
