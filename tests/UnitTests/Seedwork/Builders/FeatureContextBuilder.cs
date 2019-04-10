using Esquio.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace UnitTests.Seedwork.Builders
{
    public class FeatureContextBuilder
    {
        string _featureName = "default-feature-name";
        string _applicationName = "default-application-name";
        IServiceCollection _services = new ServiceCollection();

        public FeatureContextBuilder WithFeatureName(string featureName)
        {
            _featureName = featureName;
            return this;
        }

        public FeatureContextBuilder WithApplicationName(string applicationName)
        {
            _applicationName = applicationName;
            return this;
        }

        public FeatureContextBuilder WithService<TService, TImplementation>(TImplementation implementation)
            where TService : class
            where TImplementation : class, TService
        {
            _services.AddTransient<TService>(_ => implementation);

            return this;
        }

        public IFeatureContext Build()
        {
            return new TestingFeatureContext(_featureName, _applicationName, _services.BuildServiceProvider());
        }
    }

    class TestingFeatureContext
        : IFeatureContext
    {
        public string FeatureName { get; private set; }

        public string ApplicationName { get; private set; }

        public IServiceProvider ServiceProvider { get; private set; }

        public TestingFeatureContext(string featureName, string applicationName, IServiceProvider serviceProvider)
        {
            FeatureName = featureName;
            ApplicationName = applicationName;
            ServiceProvider = serviceProvider;
        }
    }
}
