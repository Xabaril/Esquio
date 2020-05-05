using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Products.DeleteDeployment;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Products.DeleteDeployment
{
    public class DeleteDeploymentRequestHandler : IRequestHandler<DeleteDeploymentRequest, Unit>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DeleteDeploymentRequestHandler> _logger;

        public DeleteDeploymentRequestHandler(StoreDbContext storeDbContext, ILogger<DeleteDeploymentRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteDeploymentRequest request, CancellationToken cancellationToken)
        {
            var product = await _storeDbContext
                .Products
                .Where(p => p.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (product != null)
            {
                var existingDeployment = await _storeDbContext
                    .Deployments
                    .Where(r => r.Name == request.DeploymentName)
                    .SingleOrDefaultAsync(cancellationToken);

                if (existingDeployment != null)
                {
                    if(existingDeployment.ByDefault)
                    {
                        Log.CantDeleteDefaultDeployment(_logger, request.DeploymentName, request.ProductName);
                        throw new InvalidOperationException($"The deployment {request.DeploymentName} is default deployment for product {request.ProductName} and can't be deleted.");
                    }
                    
                    _storeDbContext.Deployments
                        .Remove(existingDeployment);

                    await _storeDbContext.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }
                else
                {
                    Log.RingNotExist(_logger, request.DeploymentName, request.ProductName);
                    throw new InvalidOperationException($"The deployment {request.DeploymentName} does not exist for product {request.ProductName}");
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
