using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Products.Update
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
                .Where(p => p.Name == request.CurrentName)
                .SingleOrDefaultAsync(cancellationToken);

            if (existing != null)
            {
                existing.Name = request.Name;
                existing.Description = request.Description;

                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            Log.ProductNotExist(_logger, request.CurrentName);
            throw new InvalidOperationException($"The product {request.CurrentName} does not exist.");
        }
    }
}
