using Esquio.Abstractions;
using MediatR;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Toggles.KnownTypes
{
    public class KnownTypesToggleRequestHandler : IRequestHandler<KnownTypesToggleRequest, KnownTypesToggleResponse>
    {
        public Task<KnownTypesToggleResponse> Handle(KnownTypesToggleRequest request, CancellationToken cancellationToken)
        {
            var scaneedToggles = new List<KnownTypesToggleDetailResponse>();

            //TODO:witch assemblies?

            var assembly = Assembly.GetAssembly(typeof(Esquio.Toggles.OnToggle));

            foreach (var type in assembly.GetExportedTypes())
            {
                if (typeof(IToggle).IsAssignableFrom(type))
                {
                    var attribute = type.GetCustomAttribute<DesignTypeAttribute>();
                    var description = attribute != null ? attribute.Description : "No description";

                    scaneedToggles.Add(new KnownTypesToggleDetailResponse()
                    {
                        Type = type.FullName,
                        Assembly = assembly.GetName(copiedName: false).Name,
                        Description = description
                    });
                }
            }

            return Task.FromResult(new KnownTypesToggleResponse()
            {
                ScannedAssemblies = 1,
                Toggles = scaneedToggles
            });
        }
    }
}
