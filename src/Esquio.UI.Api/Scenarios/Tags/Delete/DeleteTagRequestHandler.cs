using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Tags.Delete;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Tags.Delete
{
    public class DeleteTagRequestHandler : IRequestHandler<DeleteTagRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DeleteTagRequestHandler> _logger;

        public DeleteTagRequestHandler(StoreDbContext storeDbContext,ILogger<DeleteTagRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteTagRequest request, CancellationToken cancellationToken)
        {
            var featureTag = await _storeDbContext.FeatureTags
                .Where(ft => ft.FeatureEntity.Name == request.FeatureName && ft.FeatureEntity.ProductEntity.Name == request.ProductName && ft.TagEntity.Name == request.Tag)
                .SingleOrDefaultAsync(cancellationToken);

            if ( featureTag != null )
            {
                _storeDbContext.Remove(featureTag);

                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            Log.FeatureTagNotExist(_logger, request.FeatureName, request.Tag);
            throw new InvalidOperationException($"The feature tag association between feature {request.FeatureName} and tag {request.Tag} does not exist.");
        }
    }
}
