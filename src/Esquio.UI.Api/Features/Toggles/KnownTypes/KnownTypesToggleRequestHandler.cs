using Esquio.Abstractions;
using Esquio.UI.Api.Infrastructure.Services;
using MediatR;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Toggles.KnownTypes
{
    public class KnownTypesToggleRequestHandler : IRequestHandler<KnownTypesToggleRequest, KnownTypesToggleResponse>
    {
        private readonly IDiscoverToggleTypesService discoverToggleTypesService;

        public KnownTypesToggleRequestHandler(IDiscoverToggleTypesService discoverToggleTypesService)
        {
            this.discoverToggleTypesService = discoverToggleTypesService ?? throw new System.ArgumentNullException(nameof(discoverToggleTypesService));
        }

        public Task<KnownTypesToggleResponse> Handle(KnownTypesToggleRequest request, CancellationToken cancellationToken)
        {
            var scaneedToggles = new List<KnownTypesToggleDetailResponse>();

            foreach (var type in discoverToggleTypesService.GetAll())
            {
                var attribute = type.GetCustomAttribute<DesignTypeAttribute>();
                var description = attribute != null ? attribute.Description : "No description";

                scaneedToggles.Add(new KnownTypesToggleDetailResponse()
                {
                    Type = type.FullName,
                    Assembly = type.Assembly.GetName(copiedName: false).Name,
                    Description = description
                });
            }

            return Task.FromResult(new KnownTypesToggleResponse()
            {
                ScannedAssemblies = 1,
                Toggles = scaneedToggles
            });
        }
    }
}
