using Esquio.Abstractions;
using MediatR;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Toggles.Known
{
    public class KnownToggleRequestHandler : IRequestHandler<KnownToggleRequest, KnownToggleResponse>
    {
        public Task<KnownToggleResponse> Handle(KnownToggleRequest request, CancellationToken cancellationToken)
        {
            var scaneedToggles = new List<KnownToggleDetailResponse>();

            //TODO:witch assemblies?

            var assembly = Assembly.GetAssembly(typeof(Esquio.Toggles.OnToggle));

            foreach (var type in assembly.GetExportedTypes())
            {
                if (typeof(IToggle).IsAssignableFrom(type))
                {
                    var attribute = type.GetCustomAttribute<DesignTypeAttribute>();
                    var description = attribute != null ? attribute.Description : "No description";

                    scaneedToggles.Add(new KnownToggleDetailResponse()
                    {
                        Type = type.FullName,
                        Assembly = assembly.FullName,
                        Description = description
                    });
                }
            }

            return Task.FromResult(new KnownToggleResponse()
            {
                ScannedAssemblies = 1,
                Toggles = scaneedToggles
            });
        }
    }
}
