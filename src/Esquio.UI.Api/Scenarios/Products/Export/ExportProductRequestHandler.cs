using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Shared.Models.Products.Export;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Products.Export
{
    public class ExportProductRequestHandler
         : IRequestHandler<ExportProductRequest, ExportProductResponse>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<ExportProductRequestHandler> _logger;

        public ExportProductRequestHandler(StoreDbContext dbContext, ILogger<ExportProductRequestHandler> logger)
        {
            _storeDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ExportProductResponse> Handle(ExportProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _storeDbContext.Products
                .Include(p => p.Deployments)
                .Include(p => p.Features)
                    .ThenInclude(f => f.Toggles)
                        .ThenInclude(t => t.Parameters)
                .Where(p => p.Name == request.ProductName)
                .SingleOrDefaultAsync();

            if (product != null)
            {
                var serialization = JsonConvert.SerializeObject(product, typeof(ProductEntity), new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new ExportContractResolver()
                });

                return new ExportProductResponse()
                {
                    Content = serialization
                };
            }

            Log.ProductNotExist(_logger, request.ProductName);
            throw new InvalidOperationException($"The product {request.ProductName} does not exist.");
        }

        private class ExportContractResolver
            : DefaultContractResolver
        {
            protected override IList<Newtonsoft.Json.Serialization.JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var defaultProperties = base.CreateProperties(type, memberSerialization);
                var properties = new List<Newtonsoft.Json.Serialization.JsonProperty>();

                foreach (var item in defaultProperties)
                {
                    if (!item.PropertyName.Contains("Id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        properties.Add(item);
                    }
                }

                return properties;
            }
        }
    }
}
