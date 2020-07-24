using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Shared.Models.Products.Import;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Products.Import
{
    public class ImportProductRequestHandler
         : IRequestHandler<ImportProductRequest, Unit>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<ImportProductRequestHandler> _logger;

        public ImportProductRequestHandler(StoreDbContext dbContext, ILogger<ImportProductRequestHandler> logger)
        {
            _storeDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(ImportProductRequest request, CancellationToken cancellationToken)
        {
            var importObject = JsonConvert.DeserializeObject<ProductEntity>(request.Content, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            var product = await _storeDbContext.Products
                .Where(p => p.Name == importObject.Name)
                .SingleOrDefaultAsync();

            if (product == null)
            {
                _storeDbContext.Products.Add(importObject);
                await _storeDbContext.SaveChangesAsync();

                return Unit.Value;
            }

            Log.ProductAlreadyExist(_logger, importObject.Name);
            throw new InvalidOperationException($"The product {importObject.Name} already exist on database.");
        }
    }
}
