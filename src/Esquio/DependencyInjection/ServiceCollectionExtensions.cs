using Esquio;
using Esquio.Abstractions;
using Esquio.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IEsquioBuilder AddEsquio(this IServiceCollection serviceCollection, Action<EsquioOptions> setup = null)
        {
            var options = new EsquioOptions();
            setup?.Invoke(options);

            options.AssembliesToRegister
                .Add(typeof(ServiceCollectionExtensions).Assembly);

            var builder = new EsquioBuilder(serviceCollection);
            builder.Services.AddScoped<IFeatureService, DefaultFeatureService>();
            builder.Services.AddScoped<IToggleTypeActivator, DefaultToggleTypeActivator>();
            builder.Services.AddTogglesFromAssemblies(options.AssembliesToRegister);

            return builder;
        }
        public static IServiceCollection AddTogglesFromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            foreach (var assembly in assemblies)
            {
                AddTogglesFromAssembly(services, assembly, lifetime);
            }

            return services;
        }
        public static IServiceCollection AddTogglesFromAssembly(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            foreach (var toggle in FindTogglesInAssembly(assembly))
            {
                AddScanResult(services, toggle, lifetime);
            }

            return services;
        }
        private static IServiceCollection AddScanResult(this IServiceCollection services, Type type, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            services.Add(
                new ServiceDescriptor(
                    serviceType: type,
                    implementationType: type,
                    lifetime: lifetime));

            return services;
        }
        private static IEnumerable<Type> FindTogglesInAssembly(Assembly assembly)
        {
            var exportedTypes = assembly.GetExportedTypes();

            return from type in exportedTypes
                   where !type.IsAbstract && typeof(IToggle).IsAssignableFrom(type)
                   select type;
        }
    }
}
