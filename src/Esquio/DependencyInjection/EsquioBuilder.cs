using Esquio.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Esquio.DependencyInjection
{
    public sealed class EsquioBuilder
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
