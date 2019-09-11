using System;
using System.Collections.Generic;

namespace Esquio.UI.Api.Infrastructure.Services
{
    public interface IDiscoverToggleTypesService
    {
        IEnumerable<Type> GetAll();
    }
}
