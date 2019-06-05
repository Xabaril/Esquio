using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api.Diagnostics;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Products.Update
{
    public class UpdateProductRequestHandler : IRequestHandler<UpdateProductRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<UpdateProductRequestHandler> _logger;

        public UpdateProductRequestHandler(StoreDbContext storeDbContext,ILogger<UpdateProductRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var existing = await _storeDbContext
                .Products
                .Where(p => p.Id == request.ProductId)
                .SingleOrDefaultAsync(cancellationToken);

            if (existing != null)
            {
                existing.Name = request.Name;
                existing.Description = request.Name;

                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            Log.ProductNotExist(_logger, request.ProductId.ToString());
            throw new InvalidOperationException($"A product with id {request.ProductId} does not exist.");
        }
    }
}
