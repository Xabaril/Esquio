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

namespace Esquio.UI.Api.Features.Flags.Rolldown
{
    internal class RolldownFlagRequestHandler
        : IRequestHandler<RolldownFlagRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<RolldownFlagRequestHandler> _logger;

        public RolldownFlagRequestHandler(StoreDbContext storeDbContext, ILogger<RolldownFlagRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Unit> Handle(RolldownFlagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
                .Where(f => f.Id == request.FeatureId)
                .Include(f => f.Toggles)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                feature.Toggles.Clear();
                feature.Toggles.Add(new ToggleEntity(feature.Id, nameof(Esquio.Toggles.OffToggle)));

                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            Log.FeatureNotExist(_logger, request.FeatureId.ToString());
            throw new InvalidOperationException("Feature does not exist in the store.");
        }
    }
}
