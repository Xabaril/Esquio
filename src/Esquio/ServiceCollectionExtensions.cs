using Esquio.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
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
            var toggleType = typeof(IToggle);

            return from type in exportedTypes
                where !type.IsAbstract && toggleType.IsAssignableFrom(type)
                select type;
        }
    }
}
