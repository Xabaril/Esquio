using Esquio.Abstractions;
using Esquio.EntityFrameworkCore.Store.Diagnostics;
using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
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

        public async Task<Feature> FindFeatureAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            Log.FindFeature(_logger, featureName, productName);

            productName = productName ?? DEFAULT_PRODUCT_NAME;

            var featureEntity = await _storeDbContext
                .Features
                .Where(f => f.Name == featureName && f.ProductEntity.Name == productName)
                .Include(f => f.Toggles)
                    .ThenInclude(t => t.Parameters)
                .SingleOrDefaultAsync(cancellationToken);

            if (featureEntity != null)
            {
                return ConvertToFeatureModel(featureEntity);
            }
            else
            {
                Log.FeatureNotExist(_logger, featureName, productName);
                return null;
            }
        }

        private Feature ConvertToFeatureModel(FeatureEntity featureEntity)
        {
            var feature = new Feature(featureEntity.Name);

            if (featureEntity.Enabled)
            {
                feature.Enabled();
            }
            else
            {
                feature.Disabled();
            }

            foreach (var toggleConfiguration in featureEntity.Toggles)
            {
                var toggle = new Toggle(toggleConfiguration.Type);

                toggle.AddParameters(toggleConfiguration
                    .Parameters
                    .Select(p => new Parameter(p.Name, GetConvertedValueFromParameter(p.ClrType, p.Value))));

                feature.AddToggle(toggle);
            }

            return feature;
        }

        private object GetConvertedValueFromParameter(string clrType, string configuredValue)
        {
            try
            {
                Log.StartingFeatureValueConversion(_logger, clrType, configuredValue);

                return TypeDescriptor.GetConverter(Type.GetType(clrType))
                    .ConvertFromInvariantString(configuredValue);
            }
            catch (Exception exception)
            {
                Log.FeatureValueConversionThrow(_logger, clrType, configuredValue, exception);

                return null;
            }
        }
    }
}
