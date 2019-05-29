using Esquio.Abstractions;
using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public RevealToggleRequestHandler(StoreDbContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            _storeDbContext = context;
        }

        public async Task<RevealToggleResponse> Handle(RevealToggleRequest request, CancellationToken cancellationToken)
        {
            var toggle = await _storeDbContext
                .Toggles
                .Where(t => t.Id == request.ToggleId)
                .SingleOrDefaultAsync(cancellationToken);

            if (toggle != null)
            {
                try
                {
                    //TODO: wich assemblies?
                    var assembly = Assembly.GetAssembly(typeof(Esquio.Toggles.OnToggle));

                    var type = assembly.GetExportedTypes()
                        .Where(type => type.FullName.Equals(toggle.Type, StringComparison.InvariantCultureIgnoreCase))
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

                            return new RevealToggleResponse()
                            {

                                Type = toggle.Type,
                                Parameters = parametersDescription
                            };
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException($"The toggle type {toggle.Type} can't be loaded and reveleaded.");
                    }
                }
                catch 
                {
                    throw new InvalidOperationException($"The toggle type {toggle.Type} can't be revealed because unexpected exception is throwed.");
                }
            }

            throw new InvalidOperationException("Toggle doent's exist in the store.");
        }
    }
}
