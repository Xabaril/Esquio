using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Toggles.Delete;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Toggles.Delete
{
    public class DeleteToggleRequestHandler : IRequestHandler<DeleteToggleRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DeleteToggleRequestHandler> _logger;

        public DeleteToggleRequestHandler(StoreDbContext storeDbContext, ILogger<DeleteToggleRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteToggleRequest request, CancellationToken cancellationToken)
        {
            var toggle = await _storeDbContext
                .Toggles
                .Include(t => t.FeatureEntity)
                .Include(t => t.Parameters)
                .Where(t => t.FeatureEntity.Name == request.FeatureName && t.FeatureEntity.ProductEntity.Name == request.ProductName && t.Type == request.ToggleType)
                .SingleOrDefaultAsync(cancellationToken);

            if (toggle != null)
            {

                _storeDbContext.Remove(toggle);
                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            Log.ToggleNotExist(_logger, request.ToggleType);
            throw new InvalidOperationException($"Toggle with id {request.ToggleType} does not exist in the store.");
        }
    }
}
