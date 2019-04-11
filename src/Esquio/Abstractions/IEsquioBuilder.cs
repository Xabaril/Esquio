using Microsoft.Extensions.DependencyInjection;

namespace Esquio.Abstractions
{
    public interface IEsquioBuilder
    {
        IServiceCollection Services { get; }
    }
}
