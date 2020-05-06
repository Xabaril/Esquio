using Esquio.Abstractions;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Toggles.Details;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Toggles.Details
{
    public class DetailsToggleRequestHandler : IRequestHandler<DetailsToggleRequest, DetailsToggleResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public DetailsToggleRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }

        public async Task<DetailsToggleResponse> Handle(DetailsToggleRequest request, CancellationToken cancellationToken)
        {
            var toggle = await _storeDbContext
                .Toggles
                .Include(t => t.Parameters)
                .Where(t => t.Type == request.ToggleType && t.FeatureEntity.Name == request.FeatureName && t.FeatureEntity.ProductEntity.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (toggle != null)
            {
                var type = Type.GetType(toggle.Type, throwOnError:false,ignoreCase:true);

                if ( type != null )
                {
                    var attribute = type.GetCustomAttribute<DesignTypeAttribute>();

                    return new DetailsToggleResponse()
                    {
                        Type = type.ShorthandAssemblyQualifiedName(),
                        Assembly = type.Assembly.GetName(copiedName: false).Name,
                        FriendlyName = attribute != null ? attribute.FriendlyName : type.Name,
                        Description = attribute != null ? attribute.Description : "No description",
                        Parameters = toggle.Parameters.Select(parameter => new ParameterDetail
                        {
                            Name = parameter.Name,
                            Value = parameter.Value,
                            DeploymentName = parameter.DeploymentName
                        }).ToList()
                    };
                }
                else
                {
                    throw new InvalidOperationException("The toggle configuration is not valid because selected Type is non known type");
                }
            }

            return null;
        }
    }
}
