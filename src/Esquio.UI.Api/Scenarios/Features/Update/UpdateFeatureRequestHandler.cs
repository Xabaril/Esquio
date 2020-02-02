using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Flags.Update
{
    public class UpdateFeatureRequestHandler : IRequestHandler<UpdateFeatureRequest, string>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<UpdateFeatureRequestHandler> _logger;

        public UpdateFeatureRequestHandler(StoreDbContext storeDbContext, ILogger<UpdateFeatureRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Handle(UpdateFeatureRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
                .Where(p => p.Name == request.CurrentName && p.ProductEntity.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                feature.Name = request.Name;
                feature.Description = request.Description;
                feature.Enabled = request.Enabled;

                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return feature.Name;
            }
            else
            {
                Log.FeatureNotExist(_logger, request.CurrentName);
                throw new InvalidOperationException($"The feature  {request.CurrentName} does not exist in the store.");
            }
        }
    }
}
