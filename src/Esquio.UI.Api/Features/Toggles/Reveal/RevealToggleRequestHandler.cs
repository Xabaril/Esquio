using Esquio.Abstractions;
using Esquio.EntityFrameworkCore.Store;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Toggles.Reveal
{
    public class RevealToggleRequestHandler : IRequestHandler<RevealToggleRequest, RevealToggleResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public RevealToggleRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }

        public Task<RevealToggleResponse> Handle(RevealToggleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                //TODO: wich assemblies?
                var assembly = Assembly.GetAssembly(typeof(Esquio.Toggles.OnToggle));

                var type = assembly.GetExportedTypes()
                    .Where(type => type.FullName.Equals(request.ToggleType, StringComparison.InvariantCultureIgnoreCase))
                    .SingleOrDefault();

                if (type != null)
                {
                    var attributes = type.GetCustomAttributes(typeof(DesignTypeParameterAttribute), inherit: true);

                    if (attributes != null && attributes.Any())
                    {
                        var parametersDescription = attributes.Select(item =>
                        {
                            var attribute = ((DesignTypeParameterAttribute)item);

                            return new RevealToggleParameterResponse()
                            {
                                Name = attribute.ParameterName,
                                ClrType = attribute.ParameterType,
                                Description = attribute.ParameterDescription
                            };
                        }).ToList();

                        return Task.FromResult(new RevealToggleResponse()
                        {

                            Type = request.ToggleType,
                            Parameters = parametersDescription
                        });
                    }
                    else
                    {
                        return Task.FromResult(new RevealToggleResponse()
                        {

                            Type = request.ToggleType,
                            Parameters = new List<RevealToggleParameterResponse>()
                        });
                    }
                }
                else
                {
                    throw new InvalidOperationException($"The toggle type {request.ToggleType} can't be loaded and reveleaded.");
                }
            }
            catch
            {
                throw new InvalidOperationException($"The toggle type {request.ToggleType} can't be revealed because unexpected exception is throwed.");
            }
        }
    }
}
