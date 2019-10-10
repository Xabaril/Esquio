using Esquio.Abstractions;
using Esquio.UI.Api.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Esquio.UI.Infrastructure.Services
{
    public class DiscoverToggleTypesService
        : IDiscoverToggleTypesService
    {
        private static IEnumerable<Type> _typesCache;

        public IEnumerable<Type> Scan()
        {
            if (_typesCache is null)
            {
                _typesCache = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Intersect(IntrospectionAssemblies)
                    .SelectMany(a => a.GetExportedTypes())
                    .Where(type => typeof(IToggle).IsAssignableFrom(type) && type.IsClass);
            }

            return _typesCache;
        }

        private static IEnumerable<Assembly> IntrospectionAssemblies = new Assembly[]
        {
            //out-of-box
            typeof(Esquio.Toggles.FromToToggle).Assembly,
            typeof(Esquio.AspNetCore.Toggles.ClaimValueToggle).Assembly,
            //esquip-contrib
            //typeof(LocationToggles.ClientIpAddressToggle).Assembly,
            //typeof(UserAgentToggles.UserAgentBrowserToggle).Assembly
            ////<--- add your custom toggle assemblies here!
        };
    }
}
