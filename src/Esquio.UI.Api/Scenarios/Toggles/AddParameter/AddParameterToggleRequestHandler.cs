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

namespace Esquio.UI.Api.Features.Toggles.AddParameter
{
    public class AddParameterToggleRequestHandler : IRequestHandler<AddParameterToggleRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<AddParameterToggleRequestHandler> _logger;

        public AddParameterToggleRequestHandler(StoreDbContext storeDbContext, ILogger<AddParameterToggleRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(AddParameterToggleRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
                .Include(f => f.Toggles)
                    .ThenInclude(t=>t.Parameters)
                .Where(f => f.Name == request.FeatureName && f.ProductEntity.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                var toggle = feature.Toggles
                    .Where(t => t.Type == request.ToggleType)
                    .SingleOrDefault();

                if (toggle != null)
                {
                    var parameter = toggle.Parameters
                        .Where(p => p.Name.Equals(request.Name))
                        .SingleOrDefault();

                    if (parameter != null)
                    {
                        parameter.Value = request.Value;
                    }
                    else
                    {
                        toggle.Parameters.Add(
                            new ParameterEntity(toggle.Id, request.Name, request.Value));
                    }

                    await _storeDbContext.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }

                Log.ToggleNotExist(_logger, request.ToggleType);
                throw new InvalidOperationException($"Toggle {request.ToggleType} does not exist on feature {request.FeatureName} for product {request.ProductName}.");
            }

            Log.FeatureNotExist(_logger, request.FeatureName);
            throw new InvalidOperationException($"Feature {request.FeatureName} does not exist on product {request.ProductName}.");
        }
    }
}
