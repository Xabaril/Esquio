using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.UI.Api.Diagnostics;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Add
{
    public class AddFlagRequestHandler : IRequestHandler<AddFlagRequest, int>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<AddFlagRequestHandler> _logger;

        public AddFlagRequestHandler(StoreDbContext storeDbContext, ILogger<AddFlagRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(AddFlagRequest request, CancellationToken cancellationToken)
        {
            var product = await _storeDbContext
                .Products
                .Where(p => p.Id == request.ProductId)
                .SingleOrDefaultAsync(cancellationToken);

            if (product != null)
            {
                var existingFeature = await _storeDbContext
                .Features
                .Include(t => t.Toggles)
                .Where(t => t.Name == request.Name)
                .SingleOrDefaultAsync(cancellationToken);

                if (existingFeature == null)
                {
                    var feature = new FeatureEntity(request.ProductId, request.Name, request.Enabled, request.Description);
                    _storeDbContext.Add(feature);

                    await _storeDbContext.SaveChangesAsync(cancellationToken);

                    return feature.Id;
                }
                else
                {
                    Log.FeatureNameAlreadyExist(_logger, request.Name);
                    throw new InvalidOperationException($"A feature with the same name already exist on the store.");
                }
            }
            else
            {
                Log.ProductNotExist(_logger, request.ProductId.ToString());
                throw new InvalidOperationException($"The product with id {request.ProductId} does not exist in the store.");
            }
        }
    }
}
