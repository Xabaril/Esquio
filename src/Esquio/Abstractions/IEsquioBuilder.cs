using Microsoft.Extensions.DependencyInjection;

namespace Esquio.Abstractions
{
    /// <summary>
    /// The builder used to register Esquio.
    /// </summary>
    public interface IEsquioBuilder
    {
        /// <summary>
        /// Gets the Microsoft.Extensions.DependencyInjection.IServiceCollection into which 
        /// Esquio services should be registered.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
