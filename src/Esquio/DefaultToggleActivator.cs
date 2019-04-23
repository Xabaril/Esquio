using Esquio.Abstractions;
using System;
using System.Reflection;

namespace Esquio
{
    public class DefaultToggleTypeActivator
        : IToggleTypeActivator
    {
        private readonly Assembly[] _assemblies;
        private readonly IServiceProvider _serviceProvider;

        public DefaultToggleTypeActivator(IServiceProvider serviceProvider)
        {
            _assemblies = AppDomain.CurrentDomain.GetAssemblies();
            _serviceProvider = serviceProvider;

            //TODO: Improve performance on this class ( IEsquioBuilder can be configure toggle assemblies and set as constructor parameter)
        }
        public IToggle CreateInstance(string toggleTypeName)
        {
            var type = Type.GetType(toggleTypeName);
            return (IToggle)_serviceProvider.GetService(type);
        }
    }
}
