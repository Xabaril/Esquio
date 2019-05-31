using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Tags.List
{
    public class ListTagRequestHandler : IRequestHandler<ListTagRequest, List<ListTagResponse>>
    {
        private readonly StoreDbContext _dbContext;

        public ListTagRequestHandler(StoreDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<ListTagResponse>> Handle(ListTagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _dbContext.GetFeatureOrThrow(request.FeatureId, cancellationToken);

            return await _dbContext
                .FeatureTagEntities
                .Include(ft => ft.TagEntity)
                .Where(ft => ft.FeatureEntityId == request.FeatureId)
                .Select(ft => new ListTagResponse(ft.TagEntity.Name))
                .ToListAsync();
        }
    }
}
