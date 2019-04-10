using System;

namespace Esquio.Abstractions
{
    public interface IFeatureContext
    {
        string FeatureName { get; }
        string ApplicationName { get; }
        IServiceProvider ServiceProvider { get; }
    }
}
