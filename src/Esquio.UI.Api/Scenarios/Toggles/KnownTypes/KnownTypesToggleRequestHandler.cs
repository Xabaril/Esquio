using Esquio.Abstractions;
using Esquio.UI.Api.Infrastructure.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Toggles.KnownTypes
{
    public class KnownTypesToggleRequestHandler : IRequestHandler<KnownTypesToggleRequest, KnownTypesToggleResponse>
    {
        private readonly IDiscoverToggleTypesService _discoverToggleTypesService;

        public KnownTypesToggleRequestHandler(IDiscoverToggleTypesService discoverToggleTypesService)
        {
            this._discoverToggleTypesService = discoverToggleTypesService ?? throw new System.ArgumentNullException(nameof(discoverToggleTypesService));
        }

        public Task<KnownTypesToggleResponse> Handle(KnownTypesToggleRequest request, CancellationToken cancellationToken)
        {
            var scaneedToggles = new List<KnownTypesToggleDetailResponse>();

            foreach (var type in _discoverToggleTypesService.Scan())
            {
                var attribute = type.GetCustomAttribute<DesignTypeAttribute>();

                scaneedToggles.Add(new KnownTypesToggleDetailResponse()
                {
                    Type = type.ShorthandAssemblyQualifiedName(),
                    Assembly = type.Assembly.GetName(copiedName: false).Name,
                    FriendlyName = attribute != null ? attribute.FriendlyName : type.Name,
                    Description = attribute != null ? attribute.Description : "No description"
                });
            }

            return Task.FromResult(new KnownTypesToggleResponse()
            {
                ScannedAssemblies = scaneedToggles.GroupBy(r => r.Assembly).Count(),
                Toggles = scaneedToggles
            });
        }
    }
}
