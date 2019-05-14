using Esquio;
using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Esquio.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides Esquio extensions methods for <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add default Esquio dependencies into the <paramref name="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/>.</param>
        /// <param name="setup">The action method to configure <see cref="EsquioOptions"/>. Optional, default is null.</param>
        /// <returns></returns>
        public static IEsquioBuilder AddEsquio(this IServiceCollection serviceCollection, Action<EsquioOptions> setup = null)
        {
            var options = new EsquioOptions();
            setup?.Invoke(options);

            var builder = new EsquioBuilder(serviceCollection);
            builder.Services.AddScoped<IFeatureService, DefaultFeatureService>();
            builder.Services.AddScoped<IToggleTypeActivator, DefaultToggleTypeActivator>();
            builder.Services.TryAddTransient<IEnvironmentNameProviderService, NoEnvironmentNameProviderService>();
            builder.Services.TryAddTransient<IUserNameProviderService, NoUserNameProviderService>();
            builder.Services.TryAddTransient<IRoleNameProviderService, NoRoleNameProviderService>();
            builder.Services.AddTogglesFromAssemblies(options.AssembliesToRegister);

            return builder;
        }
        /// <summary>
        /// Add all <see cref="IToggle"/> types from specified assemblies.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> used to register the toggles.</param>
        /// <param name="assemblies">The assemblies with custom toggles to be registered.</param>
        /// <param name="lifetime">The lifetime registration to be used.Optional, default is Transient.</param>
        /// <returns></returns>
        public static IServiceCollection AddTogglesFromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            foreach (var assembly in assemblies)
            {
                AddTogglesFromAssembly(services, assembly, lifetime);
            }

            return services;
        }
        /// <summary>
        /// Addd all <see cref="IToggle"/> types from specified assembly.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> used to register the toggles.</param>
        /// <param name="assembly">The assembly with custom toggles to be registered.</param>
        /// <param name="lifetime">The lifetime registration to be used.Optional, default is Transient.</param>
        /// <returns></returns>
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
