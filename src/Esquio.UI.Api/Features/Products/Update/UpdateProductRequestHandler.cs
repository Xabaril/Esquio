using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Products.Update
{
    public class UpdateProductRequestHandler : IRequestHandler<UpdateProductRequest>
    {
        private readonly StoreDbContext _dbContext;

        public UpdateProductRequestHandler(StoreDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var existing = await _dbContext
                .Products
                .Where(p => p.Id == request.ProductId)
                .SingleOrDefaultAsync(cancellationToken);

            if (existing != null)
            {
                existing.Name = request.Name;
                existing.Description = request.Name;

                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            throw new InvalidOperationException($"A product with id {request.ProductId} does not exist.");
        }
    }
}
