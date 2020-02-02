using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Flags.Add
{
    public class AddFeatureRequestHandler : IRequestHandler<AddFeatureRequest, string>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<AddFeatureRequestHandler> _logger;

        public AddFeatureRequestHandler(StoreDbContext storeDbContext, ILogger<AddFeatureRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Handle(AddFeatureRequest request, CancellationToken cancellationToken)
        {
            var product = await _storeDbContext
                .Products
                .Where(p => p.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (product != null)
            {
                var existingFeature = await _storeDbContext
                    .Features
                    .Where(f => f.Name == request.Name && f.ProductEntityId == product.Id)
                    .SingleOrDefaultAsync(cancellationToken);

                if (existingFeature == null)
                {
                    var feature = new FeatureEntity(product.Id, request.Name, request.Enabled, request.Archived, request.Description);
                    _storeDbContext.Add(feature);

                    await _storeDbContext.SaveChangesAsync(cancellationToken);

                    return feature.Name;
                }
                else
                {
                    Log.FeatureNameAlreadyExist(_logger, request.Name);
                    throw new InvalidOperationException($"A feature with the same name already exist on the store.");
                }
            }
            else
            {
                Log.ProductNotExist(_logger, request.ProductName);
                throw new InvalidOperationException($"The product with id {request.ProductName} does not exist in the store.");
            }
        }
    }
}
