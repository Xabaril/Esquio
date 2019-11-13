using Esquio.Abstractions;
using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Details
{
    public class DetailsFlagRequestHandler : IRequestHandler<DetailsFlagRequest, DetailsFlagResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public DetailsFlagRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        public async Task<DetailsFlagResponse> Handle(DetailsFlagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
               .Features
               .Where(f => f.ProductEntity.Name == request.ProductName && f.Name == request.FeatureName)
               .Include(f => f.ProductEntity)
               .Include(f => f.Toggles)
                .ThenInclude(t=>t.Parameters)
               .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                return new DetailsFlagResponse()
                {
                    Name = feature.Name,
                    Description = feature.Description,
                    Enabled = feature.Enabled,
                    Toggles = feature.Toggles.Select(toggle =>
                    {
                        var type = Type.GetType(toggle.Type);
                        var designTypeAttribute = type?.GetCustomAttribute<DesignTypeAttribute>();

                        var parameters = toggle.Parameters
                            .Select(p => new ParameterDetail()
                            {
                                Name = p.Name,
                                Value = p.Value
                            }).ToList();

                        return new ToggleDetail()
                        {
                            Id = toggle.Id,
                            Type = toggle.Type,
                            FriendlyName = designTypeAttribute?.FriendlyName ?? toggle.Type,
                            Parameters = parameters
                        };

                    }).ToList()
                };
            }

            return null;
        }
    }
}
