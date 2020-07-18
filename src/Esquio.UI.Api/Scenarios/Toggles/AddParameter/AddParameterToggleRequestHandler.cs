using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Toggles.AddParameter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Toggles.AddParameter
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
                .Include(f => f.ProductEntity)  //-> this is only needed for "history"
                .Include(f => f.Toggles)
                    .ThenInclude(t => t.Parameters)
                .Where(f => f.Name == request.FeatureName && f.ProductEntity.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                var toggle = feature.Toggles
                    .Where(t => t.Type == request.ToggleType)
                    .SingleOrDefault();

                if (toggle != null)
                {
                    var allowedDeployments = await _storeDbContext
                        .Deployments
                        .Where(p => p.ProductEntityId == feature.ProductEntityId)
                        .ToListAsync();

                    var defaultDeployment = allowedDeployments
                           .Where(r => r.ByDefault)
                           .SingleOrDefault();

                    var selectedDeployment = defaultDeployment;

                    if (!string.IsNullOrEmpty(request.DeploymentName))
                    {
                        selectedDeployment = allowedDeployments
                           .Where(r => r.Name == request.DeploymentName)
                           .SingleOrDefault();
                    }

                    if (selectedDeployment != null)
                    {
                        toggle.AddOrUpdateParameter(selectedDeployment, defaultDeployment, request.Name, request.Value);

                        await _storeDbContext.SaveChangesAsync(cancellationToken);
                        return Unit.Value;
                    }

                    Log.RingNotExist(_logger, request.DeploymentName, request.ProductName);
                    throw new InvalidOperationException($"Deployment {request.DeploymentName} does not exist for product {request.ProductName}.");
                }

                Log.ToggleNotExist(_logger, request.ToggleType);
                throw new InvalidOperationException($"Toggle {request.ToggleType} does not exist on feature {request.FeatureName} for product {request.ProductName}.");
            }

            Log.FeatureNotExist(_logger, request.FeatureName);
            throw new InvalidOperationException($"Feature {request.FeatureName} does not exist on product {request.ProductName}.");
        }
    }
}
