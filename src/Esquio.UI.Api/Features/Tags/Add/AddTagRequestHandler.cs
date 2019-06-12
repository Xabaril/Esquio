using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Tags.Add
{
    public class AddTagRequestHandler : IRequestHandler<AddTagRequest>
    {
        private readonly StoreDbContext _dbContext;

        public AddTagRequestHandler(StoreDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(AddTagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _dbContext.GetFeatureOrThrow(request.FeatureId, cancellationToken);

            var tag = await _dbContext
                .Tags
                .SingleOrDefaultAsync(t => t.Name == request.Tag);

            if (tag == null)
            {
                tag = new TagEntity(request.Tag);
                await _dbContext.Tags.AddAsync(tag);
            }

            var featureTag = await _dbContext.GetFeatureTagBy(
                request.FeatureId,
                request.Tag,
                cancellationToken);

            if (featureTag != null)
            {
                throw new InvalidOperationException($"Feature has been tagged with the tag {request.Tag}");
            }

            featureTag = new FeatureTagEntity
            {
                FeatureEntityId = feature.Id,
                TagEntityId = tag.Id
            };

            await _dbContext.FeatureTagEntities.AddAsync(featureTag);

            return Unit.Value;
        }
    }
}
