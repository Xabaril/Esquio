using Esquio.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Esquio.DependencyInjection
{

    /// <summary>
    /// Provides programatic configuration for Esquio services.
    /// </summary>
    public class EsquioOptions
    {
        internal List<Assembly> AssembliesToRegister { get; } = new List<Assembly>
        {
            typeof(IToggle).Assembly
        };

        /// <summary>
        /// Register custom <see cref="Esquio.Abstractions.IToggle"/> defined in assembly on wich <typeparamref name="T"/> is defined.
        /// </summary>
        /// <typeparam name="T">The type defined on assembly to be added.</typeparam>
        /// <returns>The same configuration to be chained.</returns>
        public EsquioOptions RegisterTogglesFromAssemblyContaining<T>()
        {
            return RegisterTogglesFromAssemblyContaining(typeof(T));
        }

        // <summary>
        /// Register custom <see cref="Esquio.Abstractions.IToggle"/> defined in assembly on wich <param name="type"/> is defined.
        /// </summary>
        /// <param name="type">The type defined on assembly to be added.</param>
        /// <returns>The same configuration to be chained.</returns>
        public EsquioOptions RegisterTogglesFromAssemblyContaining(Type type)
        {
            return RegisterTogglesFromAssembly(type.GetTypeInfo().Assembly);
        }

        /// <summary>
        /// Register custom  <see cref="Esquio.Abstractions.IToggle"/> defined in <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly that contain custom <see cref="Esquio.Abstractions.IToggle"/>.</param>
        /// <returns>The same configuration to be chained.</returns>
        public EsquioOptions RegisterTogglesFromAssembly(Assembly assembly)
        {
            AssembliesToRegister.Add(assembly);
            return this;
        }

        /// Register custom  <see cref="Esquio.Abstractions.IToggle"/> defined in <paramref name="assemblies"/>.
        /// </summary>
        /// <param name="assemblies">A collection of  assemblies that contain custom <see cref="Esquio.Abstractions.IToggle"/>.</param>
        /// <returns>The same configuration to be chained.</returns>
        public EsquioOptions RegisterTogglesFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            AssembliesToRegister.AddRange(assemblies);
            return this;
        }
    }
}
