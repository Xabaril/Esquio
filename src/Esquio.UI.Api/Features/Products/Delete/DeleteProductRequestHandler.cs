using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Products.Delete
{
    public class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequest>
    {
        private readonly StoreDbContext _storeDbContext;

        public DeleteProductRequestHandler(StoreDbContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            _storeDbContext = context;
        }
        public async Task<Unit> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _storeDbContext
                .Products
                .Where(p=>p.Id == request.ProductId)
                .SingleOrDefaultAsync(cancellationToken);

            if (product != null)
            {
                _storeDbContext.Remove(product);

                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            throw new InvalidOperationException($"The product with identifier {request.ProductId} does not exist.");
        }
    }
}
