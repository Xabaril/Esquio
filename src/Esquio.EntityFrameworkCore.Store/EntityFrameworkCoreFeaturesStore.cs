using Esquio.Abstractions;
using Esquio.EntityFrameworkCore.Store.Diagnostics;
using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.EntityFrameworkCore.Store
{
    internal class EntityFrameworkCoreFeaturesStore
        : IRuntimeFeatureStore
    {
        const string DEFAULT_PRODUCT_NAME = "default";

        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<EntityFrameworkCoreFeaturesStore> _logger;

        public EntityFrameworkCoreFeaturesStore(StoreDbContext storeDbContext, ILogger<EntityFrameworkCoreFeaturesStore> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Feature> FindFeatureAsync(string featureName, string productName = null)
        {
            Log.FindFeature(_logger, featureName, productName);

            productName = productName ?? DEFAULT_PRODUCT_NAME;

            var featureEntity = await _storeDbContext
                .Features
                .Where(f => f.Name == featureName && f.ProductEntity.Name == productName)
                .Include(f => f.Toggles)
                    .ThenInclude(t => t.Parameters)
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
