using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api.Diagnostics;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Rollout
{
    internal class RolloutFlagRequestHandler
        : IRequestHandler<RolloutFlagRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<RolloutFlagRequestHandler> _logger;

        public RolloutFlagRequestHandler(StoreDbContext storeDbContext, ILogger<RolloutFlagRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Unit> Handle(RolloutFlagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
                .Where(f => f.Id == request.FeatureId)
                .Include(f => f.Toggles)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                feature.Enabled = true;

                if (feature.Toggles.Any())
                {
                    feature.Toggles.Clear();
                }

                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            Log.FeatureNotExist(_logger, request.FeatureId.ToString());
            throw new InvalidOperationException("Feature does not exist in the store.");
        }
    }
}
