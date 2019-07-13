using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.DetailsExtended
{
    public class DetailsExtendedFlagRequestHandler : IRequestHandler<DetailsExtendedFlagRequest, DetailsExtendedFlagResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public DetailsExtendedFlagRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        public async Task<DetailsExtendedFlagResponse> Handle(DetailsExtendedFlagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
               .Features
               .Where(f => f.Id == request.FeatureId)
               .Include(f => f.ProductEntity)
               .Include(f => f.Toggles)
               .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                return new DetailsExtendedFlagResponse()
                {
                    Id = feature.Id,
                    Name = feature.Name,
                    Description = feature.Description,
                    Enabled = feature.Enabled,
                    ProductName = feature.ProductEntity.Name,
                    Toggles = feature.Toggles.Select(toggle => new ToggleDetail
                    {
                        Id = toggle.Id,
                        Type = toggle.Type
                    }).ToList()
                };
            }

            return null;
        }
    }
}
