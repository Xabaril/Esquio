using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Shared.Models.Products.AddDeployment;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Products.AddDeployment
{
    public class AddDeploymentRequestHandler : IRequestHandler<AddDeploymentRequest, Unit>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<AddDeploymentRequestHandler> _logger;

        public AddDeploymentRequestHandler(StoreDbContext storeDbContext, ILogger<AddDeploymentRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(AddDeploymentRequest request, CancellationToken cancellationToken)
        {
            var existing = await _storeDbContext
                .Products
                .Where(p => p.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (existing != null)
            {
                var existingDeployment = await _storeDbContext
                    .Deployments
                    .Where(r => r.Name == request.Name && r.ProductEntity.Name == request.ProductName)
                    .SingleOrDefaultAsync(cancellationToken);

                if (existingDeployment == null)
                {
                    var deployment = new DeploymentEntity(
                        productEntityId: existing.Id,
                        name: request.Name,
                        byDefault: false);

                    _storeDbContext.Deployments
                        .Add(deployment);

                    await _storeDbContext.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }
                else
                {
                    Log.DeploymentAlreadyExist(_logger, request.Name, request.ProductName);
                    throw new InvalidOperationException($"The deployment {request.Name} already exist for product {request.ProductName}");
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
