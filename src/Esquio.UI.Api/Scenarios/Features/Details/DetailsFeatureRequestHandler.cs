using Esquio.Abstractions;
using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Features.Details;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Flags.Details
{
    public class DetailsFeatureRequestHandler : IRequestHandler<DetailsFeatureRequest, DetailsFeatureResponse>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DetailsFeatureRequestHandler> _logger;

        public DetailsFeatureRequestHandler(StoreDbContext storeDbContext, ILogger<DetailsFeatureRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DetailsFeatureResponse> Handle(DetailsFeatureRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
               .Features
               .Where(f => f.ProductEntity.Name == request.ProductName && f.Name == request.FeatureName)
               .Include(f => f.ProductEntity)
               .Include(f => f.Toggles)
                .ThenInclude(t=>t.Parameters)
               .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                return new DetailsFeatureResponse()
                {
                    Name = feature.Name,
                    Description = feature.Description,
                    Toggles = feature.Toggles.Select(toggle =>
                    {
                        var type = Type.GetType(toggle.Type);
                        var designTypeAttribute = type?.GetCustomAttribute<DesignTypeAttribute>();

                        var parameters = toggle.Parameters
                            .Select(p => new ParameterDetail()
                            {
                                Name = p.Name,
                                Value = p.Value
                            }).ToList();

                        return new ToggleDetail()
                        {
                            Type = toggle.Type,
                            FriendlyName = designTypeAttribute?.FriendlyName ?? toggle.Type,
                            Parameters = parameters
                        };

                    }).ToList()
                };
            }

            Log.FeatureNotExist(_logger, request.FeatureName);
            return null;
        }
    }
}
