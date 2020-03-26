using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Tags.List;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Tags.List
{
    public class ListTagRequestHandler : IRequestHandler<ListTagRequest, IEnumerable<TagResponseDetail>>
    {
        private readonly StoreDbContext _dbContext;
        private readonly ILogger<ListTagRequestHandler> _logger;

        public ListTagRequestHandler(StoreDbContext dbContext, ILogger<ListTagRequestHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TagResponseDetail>> Handle(ListTagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _dbContext.Features
                .Include(f => f.FeatureTags)
                .ThenInclude(ft => ft.TagEntity)
                .Where(f => f.Name == request.FeatureName && f.ProductEntity.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                return feature.FeatureTags
                    .Select(ft => new TagResponseDetail(ft.TagEntity.Name))
                    .ToList();
            }

            Log.FeatureNotExist(_logger, request.FeatureName);
            throw new InvalidOperationException($"The feature with id {request.FeatureName} does not exist in the store.");
        }
    }
}
