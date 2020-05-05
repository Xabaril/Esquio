using Esquio.Abstractions;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Features.Details;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Flags.Details
{
    public class DetailsFeatureRequestHandler : IRequestHandler<DetailsFeatureRequest, DetailsFeatureResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public DetailsFeatureRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        public async Task<DetailsFeatureResponse> Handle(DetailsFeatureRequest request, CancellationToken cancellationToken)
        {
            //TODO: details not need parameters information we can reduce complexibility here!

            var feature = await _storeDbContext
               .Features
               .Where(f => f.ProductEntity.Name == request.ProductName && f.Name == request.FeatureName)
               .Include(f=>f.FeatureStates)
               .Include(f => f.ProductEntity)
               .Include(f => f.Toggles)
                .ThenInclude(t=>t.Parameters)
               .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                var rings = await _storeDbContext
                    .Rings
                    .Where(r => r.ProductEntityId == feature.ProductEntityId)
                    .ToListAsync();

                return new DetailsFeatureResponse()
                {
                    Name = feature.Name,
                    Description = feature.Description,
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
                            Type = toggle.Type,
                            FriendlyName = designTypeAttribute?.FriendlyName ?? toggle.Type,
                            Parameters = parameters
                        };

                    }).ToList(),
                    States = rings.Select(r=>new FeatureStateDetail()
                    {
                        RingName = r.Name,
                        Enabled = feature.FeatureStates.Where(fs=>fs.RingEntityId==r.Id).SingleOrDefault()?.Enabled ?? false
                    }).ToList()
                };
            }

            //TODO: add diagnostics here!

            return null;
        }
    }
}
