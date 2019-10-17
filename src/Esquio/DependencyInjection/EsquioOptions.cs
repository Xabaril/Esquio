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
        internal List<Assembly> AssembliesToRegister { get; private set; } = new List<Assembly>
        {
            typeof(IToggle).Assembly
        };

        internal string DefaultProductName = null;

        /// <summary>
        /// Configure default product name to be used when product parameter is not specified. This 
        /// override default product name configured on <see cref="EsquioConstants.DEFAULT_PRODUCT_NAME"/>
        /// </summary>
        /// <param name="productName">The product name to be used when the "Product" parameter is not stablished.</param>
        /// <returns></returns>
        public EsquioOptions ConfigureDefaultProductName(string productName)
        {
            DefaultProductName = productName;
            return this;
        }

        internal OnErrorBehavior OnErrorBehavior { get; set; } = OnErrorBehavior.SetDisabled;
        /// <summary>
        /// Configure default <see cref="OnErrorBehavior"/> to used when feature evaluation throw an exception. Default value is SetDisabled.
        /// </summary>
        /// <param name="onErrorBehavior">The <see cref="OnErrorBehavior"/> to configure as default.</param>
        /// <returns>The same configuration to be chained.</returns>
        public EsquioOptions ConfigureOnErrorBehavior(OnErrorBehavior onErrorBehavior)
        {
            OnErrorBehavior = onErrorBehavior;
            return this;
        }
        internal NotFoundBehavior NotFoundBehavior { get; set; } = NotFoundBehavior.SetDisabled;
        /// <summary>
        /// Configure default <see cref="NotFoundBehavior"/> to used when feature to evaluate not exist in the store. Default value is SetDisabled.
        /// </summary>
        /// <param name="notFoundBehavior">The <see cref="NotFoundBehavior"/> to configure as default.</param>
        /// <returns>The same configuration to be chained.</returns>
        public EsquioOptions ConfigureNotFoundBehavior(NotFoundBehavior notFoundBehavior)
        {
            NotFoundBehavior = notFoundBehavior;
            return this;
        }
        /// <summary>
        /// Register custom <see cref="Esquio.Abstractions.IToggle"/> defined in assembly on wich <typeparamref name="T"/> is defined.
        /// </summary>
        /// <typeparam name="T">The type defined on assembly to be added.</typeparam>
        /// <returns>The same configuration to be chained.</returns>
        public EsquioOptions RegisterTogglesFromAssemblyContaining<T>()
        {
            return RegisterTogglesFromAssemblyContaining(typeof(T));
        }
        /// <summary>
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
        /// <summary>
        /// Register custom <see cref="Esquio.Abstractions.IToggle"/> defined in <paramref name="assemblies"/>.
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
