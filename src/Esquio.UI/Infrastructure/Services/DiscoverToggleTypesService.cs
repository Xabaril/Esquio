using Esquio.Abstractions;
using Esquio.UI.Api.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.UI.Infrastructure.Services
{
    public class DiscoverToggleTypesService : IDiscoverToggleTypesService
    {
        public IEnumerable<Type> GetAll()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a =>
                    a.Equals(typeof(Toggles.OnToggle).Assembly)
                    || a.Equals(typeof(AspNetCore.Toggles.ClaimValueToggle).Assembly))
                .SelectMany(a => a.GetExportedTypes())
                .Where(type => typeof(IToggle).IsAssignableFrom(type));
        }
    }
}
