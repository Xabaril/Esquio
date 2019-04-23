using Esquio.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Esquio
{
    public class EsquioOptions
    {
        public List<Assembly> AssembliesToRegister { get; } = new List<Assembly>
        {
            typeof(IToggle).Assembly
        };

        public EsquioOptions RegisterTogglesFromAssemblyContaining<T>()
        {
            return RegisterTogglesFromAssemblyContaining(typeof(T));
        }

        public EsquioOptions RegisterTogglesFromAssemblyContaining(Type type)
        {
            return RegisterTogglesFromAssembly(type.GetTypeInfo().Assembly);
        }

        public EsquioOptions RegisterTogglesFromAssembly(Assembly assembly)
        {
            AssembliesToRegister.Add(assembly);
            return this;
        }

        public EsquioOptions RegisterTogglesFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            AssembliesToRegister.AddRange(assemblies);
            return this;
        }
    }
}
