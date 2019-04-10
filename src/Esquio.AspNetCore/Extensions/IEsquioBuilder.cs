using Microsoft.Extensions.DependencyInjection;
using System;

namespace Esquio.AspNetCore.Extensions
{
    public interface IEsquioBuilder
    {
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
