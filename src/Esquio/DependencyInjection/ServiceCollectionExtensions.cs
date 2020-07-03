using Esquio;
using Esquio.Abstractions;
using Esquio.DependencyInjection;
using Esquio.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

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
        /// <returns>The <see cref="IEsquioBuilder"/> used to configure Esquio.</returns>
        public static IEsquioBuilder AddEsquio(this IServiceCollection serviceCollection, Action<EsquioOptions> setup = null)
        {
            var options = new EsquioOptions();
            setup?.Invoke(options);

            var builder = new EsquioBuilder(serviceCollection);
            builder.Services.Configure<EsquioOptions>(opt =>
            {
                opt.OnErrorBehavior = options.OnErrorBehavior;
                opt.NotFoundBehavior = options.NotFoundBehavior;
                opt.DefaultProductName = options.DefaultProductName;
                opt.ScopedEvaluationEnabled = options.ScopedEvaluationEnabled;
                opt.DefaultDeploymentName = options.DefaultDeploymentName;
            });

            builder.Services.AddScoped<IFeatureService, DefaultFeatureService>();
            builder.Services.AddScoped<IToggleTypeActivator, DefaultToggleTypeActivator>();

            //allow to replace this services and not fix the order
            builder.Services.TryAddScoped<IScopedEvaluationHolder, NoScopedEvaluationHolder>();
            builder.Services.TryAddSingleton<IValuePartitioner, DefaultValuePartitioner>();

            builder.Services.AddSingleton<EsquioDiagnostics>();
            builder.Services.AddTogglesFromAssemblies(options.AssembliesToRegister);

            return builder;
        }
        /// <summary>
        /// Add all <see cref="IToggle"/> types from specified assemblies.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> used to register the toggles.</param>
        /// <param name="assemblies">The assemblies with custom toggles to be registered.</param>
        /// <param name="lifetime">The lifetime registration to be used.Optional, default is Transient.</param>
        /// <returns>The collection of configured services.</returns>
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
        /// <returns>The collection of configured services.</returns>
        public static IServiceCollection AddTogglesFromAssembly(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            foreach (var toggle in FindTogglesInAssembly(assembly))
            {
                AddScanResult(services, toggle, lifetime);
            }

            return services;
        }

        /// <summary>
        /// Add a new service depending on a feature state.
        /// </summary>
        /// <typeparam name="TService">The system type of the service.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> used.</param>
        /// <param name="featureName">The feature name to be evaluated.</param>
        /// <param name="featureEnabled">The factory used to create the service implementation when the feature is enabled.</param>
        /// <param name="featureDisabled">The factory used to create the service implementation when the feature is disabled.</param>
        /// <param name="serviceLifetime">The service lifetime to set.</param>
        /// <returns>The collection of configured services.</returns>
        public static IServiceCollection Add<TService>(this IServiceCollection services,
            string featureName,
            Func<IServiceProvider, object> featureEnabled,
            Func<IServiceProvider, object> featureDisabled,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            _ = featureName ?? throw new ArgumentNullException(nameof(featureName));
            _ = featureEnabled ?? throw new ArgumentNullException(nameof(featureEnabled));
            _ = featureDisabled ?? throw new ArgumentNullException(nameof(featureDisabled));

            var descriptor = new ServiceDescriptor(typeof(TService), serviceProvider =>
            {
                var featureService = serviceProvider.GetService<IFeatureService>();

                var enabled = featureService.IsEnabledAsync(featureName)
                    .GetAwaiter()
                    .GetResult();

                return enabled ? featureEnabled(serviceProvider) : featureDisabled(serviceProvider);
            }, serviceLifetime);

            services.Add(descriptor);

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
