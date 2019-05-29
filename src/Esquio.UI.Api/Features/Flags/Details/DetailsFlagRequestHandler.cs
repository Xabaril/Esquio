using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Details
{
    public class DetailsFlagRequestHandler : IRequestHandler<DetailsFlagRequest, DetailsFlagResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public DetailsFlagRequestHandler(StoreDbContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            _storeDbContext = context;
        }
        public async Task<DetailsFlagResponse> Handle(DetailsFlagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
               .Features
               .Include(f=>f.ProductEntity)
               .Include(f => f.Toggles)
               .SingleOrDefaultAsync(f => f.Id == request.FeatureId && f.ProductEntityId == request.ProductId, cancellationToken);

            if (feature != null)
            {
                return new DetailsFlagResponse()
                {
                    Id = feature.Id,
                    Name = feature.Name,
                    Enabled = feature.Enabled,
                    ProductName = feature.ProductEntity.Name,
                    Toggles = feature.Toggles.Select(t => t.Type).ToList()
                };
            }

            return null;
        }
    }
}
