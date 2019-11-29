using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api.Diagnostics;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Products.Delete
{
    public class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DeleteProductRequestHandler> _logger;

        public DeleteProductRequestHandler(StoreDbContext storeDbContext, ILogger<DeleteProductRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(logger));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Unit> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _storeDbContext
                .Products

                .Where(p => p.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (product != null)
            {
                _storeDbContext.Remove(product);

                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            Log.ProductNotExist(_logger, request.ProductName);
            throw new InvalidOperationException($"The product {request.ProductName} does not exist.");
        }
    }
}
