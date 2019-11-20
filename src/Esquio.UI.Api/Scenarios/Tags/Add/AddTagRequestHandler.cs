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

namespace Esquio.UI.Api.Features.Tags.Add
{
    public class AddTagRequestHandler : IRequestHandler<AddTagRequest>
    {
        private readonly StoreDbContext _dbContext;
        private readonly ILogger<AddTagRequestHandler> _logger;

        public AddTagRequestHandler(StoreDbContext dbContext, ILogger<AddTagRequestHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(AddTagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _dbContext
                .Features
                .Where(f => f.Name == request.FeatureName && f.ProductEntity.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                var tag = await _dbContext
                    .Tags
                    .SingleOrDefaultAsync(t => t.Name == request.Tag);

                if (tag == null)
                {
                    tag = new TagEntity(request.Tag);
                }

                var featureTag = new FeatureTagEntity()
                {
                    FeatureEntityId = feature.Id,
                    TagEntity = tag
                };

                var alreadyExist = await _dbContext.FeatureTagEntities
                    .AnyAsync(ft => ft.FeatureEntityId == feature.Id && ft.TagEntityId == tag.Id, cancellationToken);

                if ( !alreadyExist )
                {
                    await _dbContext.FeatureTagEntities
                    .AddAsync(featureTag);

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }
                else
                {
                    Log.FeatureTagAlreadyExist(_logger, request.FeatureName, request.Tag);
                    throw new InvalidOperationException($"Tag already exist for this feature");
                }
            }

            Log.FeatureNotExist(_logger, request.FeatureName);
            throw new InvalidOperationException($"The feature with id {request.FeatureName} does not exist in the store.");
        }
    }
}
