using Microsoft.Extensions.DependencyInjection;
using System;

namespace Esquio.DependencyInjection
{
    /// <summary>
    /// The builder used to register Esquio and dependant services.
    /// </summary>
    public interface IEsquioBuilder
    {
        /// <summary>
        /// Gets the Microsoft.Extensions.DependencyInjection.IServiceCollection into which 
        /// Esquio services should be registered.
        /// </summary>
        IServiceCollection Services { get; }
    }

    internal sealed class EsquioBuilder
        : IEsquioBuilder
    {
        public IServiceCollection Services
        {
            get;
        }

        public EsquioBuilder(IServiceCollection serviceCollection)
        {
            Services = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
        }
    }
}
