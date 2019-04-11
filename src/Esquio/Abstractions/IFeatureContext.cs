using System;

namespace Esquio.Abstractions
{
    /// <summary>
    /// Represent the feature validation context to be used on toggles to
    /// acquire registered services and get the information of the application 
    /// and feaeture that need validate the toggle.
    /// </summary>
    public interface IFeatureContext
    {
        /// <summary>
        /// Get the feature name on this context.
        /// </summary>
        string FeatureName { get; }
        /// <summary>
        /// Get the application name on this context.
        /// </summary>
        string ApplicationName { get; }
        /// <summary>
        /// Get the <see cref="System.IServiceProvider"/> when all registered services are availabe.
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }
}
