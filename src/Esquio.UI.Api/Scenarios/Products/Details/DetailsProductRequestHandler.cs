using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Products.Details
{
    public class DetailsProductRequestHandler : IRequestHandler<DetailsProductRequest, DetailsProductResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public DetailsProductRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        
        public async Task<DetailsProductResponse> Handle(DetailsProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _storeDbContext
               .Products
               .Where(f => f.Name == request.ProductName)
               .SingleOrDefaultAsync(cancellationToken);

            if (product != null)
            {
                return new DetailsProductResponse()
                {
                    Name = product.Name,
                    Description = product.Description
                };
            }

            return null;
        }
    }
}
