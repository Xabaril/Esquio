using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Tags.Delete
{
    public class DeleteTagRequestHandler : IRequestHandler<DeleteTagRequest>
    {
        private readonly StoreDbContext _storeDbContext;

        public DeleteTagRequestHandler(StoreDbContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            _storeDbContext = context;
        }

        public async Task<Unit> Handle(DeleteTagRequest request, CancellationToken cancellationToken)
        {
            var featureTag = await _storeDbContext
                .FeatureTagEntities
                .Include(ft => ft.TagEntity)
                .SingleOrDefaultAsync(ft => ft.FeatureEntityId == request.FeatureId && ft.TagEntity.Name == request.Tag, cancellationToken);

            if (featureTag != null)
            {
                _storeDbContext.Remove(featureTag);
                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            throw new InvalidOperationException($"There is no tagged feature with the tag {request.Tag}");
        }
    }
}
