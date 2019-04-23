using Esquio.Abstractions;
using System;

namespace Esquio.AspNetCore.Providers
{
    internal class AspNetCoreFeatureContextFactory
        : IFeatureContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AspNetCoreFeatureContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
        public IFeatureContext Create(string applicationName, string featureName)
        {
            return new AspNetCoreFeatureContext(applicationName, featureName, _serviceProvider);
        }
    }
    internal class AspNetCoreFeatureContext
        : IFeatureContext
    {
        public string FeatureName { get; }

        public string ApplicationName { get; }

        public IServiceProvider ServiceProvider { get; }

        internal AspNetCoreFeatureContext(string applicationName, string featureName, IServiceProvider serviceProvider)
        {
            ApplicationName = applicationName;
            FeatureName = featureName;
            ServiceProvider = serviceProvider;
        }
    }
}
