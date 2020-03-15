using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Products.DetailsRing
{
    public class DetailsRingRequestHandler : IRequestHandler<DetailsRingRequest, DetailsRingResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public DetailsRingRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        
        public async Task<DetailsRingResponse> Handle(DetailsRingRequest request, CancellationToken cancellationToken)
        {
            var product = await _storeDbContext
               .Products
               .Where(p => p.Name == request.ProductName)
               .Include(p=>p.Rings)
               .SingleOrDefaultAsync(cancellationToken);

            if (product != null)
            {
                return new DetailsRingResponse()
                {
                    ProductName = request.ProductName,
                    Rings = product.Rings.Select(r => new DetailsRingResponseDetail()
                    {
                        RingName = r.Name,
                        Default = r.ByDefault
                    })
                };
            }

            return null;
        }
    }
}
