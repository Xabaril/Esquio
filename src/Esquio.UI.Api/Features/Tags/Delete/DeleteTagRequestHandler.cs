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
            var featureTag = await _storeDbContext.GetFeatureTagOrThrow(
                request.FeatureId,
                request.Tag,
                cancellationToken);

            _storeDbContext.Remove(featureTag);

            await _storeDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
