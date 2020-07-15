using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Shared.Models.Products.Add;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Products.Add
{
    public class AddProductRequestHandler : IRequestHandler<AddProductRequest, string>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<AddProductRequestHandler> _logger;

        public AddProductRequestHandler(StoreDbContext storeDbContext, ILogger<AddProductRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Handle(AddProductRequest request, CancellationToken cancellationToken)
        {
            var existing = await _storeDbContext
                .Products
                .Where(p => p.Name == request.Name)
                .SingleOrDefaultAsync(cancellationToken);

            if (existing == null)
            {
                var product = new ProductEntity(request.Name, request.Description);

                product.Deployments.Add(new DeploymentEntity(
                    productEntityId: product.Id,
                    name: request.DefaultDeploymentName ?? EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                    byDefault: true));

                _storeDbContext.Add(product);

                await _storeDbContext
                .SaveChangesAsync(cancellationToken);

                return product.Name;
            }

            Log.ProductAlreadyExist(_logger, request.Name);
            throw new InvalidOperationException("A product with the same name already exist in the store.");
        }
    }
}
