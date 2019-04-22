using Esquio.Abstractions;
using System;
using System.Reflection;

namespace Esquio
{
    public class DefaultToggleTypeActivator
        : IToggleTypeActivator
    {
        private readonly Assembly[] _assemblies;

        public DefaultToggleTypeActivator()
        {
            _assemblies = AppDomain.CurrentDomain.GetAssemblies();

            //TODO: Improve performance on this class ( IEsquioBuilder can be configure toggle assemblies and set as constructor parameter)
        }
        public IToggle CreateInstance(string toggleTypeName)
        {
            foreach (var assembly in _assemblies)
            {
                var type = assembly.GetType(toggleTypeName);

                if (type != null)
                {
                    return (IToggle)Activator.CreateInstance(type);
                }
            }

            return default;
        }
    }
}
