using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Products.Add
{
    public class AddProductRequestHandler : IRequestHandler<AddProductRequest, int>
    {
        private readonly StoreDbContext _dbContext;

        public AddProductRequestHandler(StoreDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<int> Handle(AddProductRequest request, CancellationToken cancellationToken)
        {
            var existing = await _dbContext
                .Products
                .Where(p => p.Name == request.Name)
                .SingleOrDefaultAsync(cancellationToken);

            if (existing == null)
            {
                var product = new ProductEntity(request.Name, request.Description);
                _dbContext.Add(product);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return product.Id;
            }

            throw new InvalidOperationException("A product with the same name already exist.");
        }
    }
}
