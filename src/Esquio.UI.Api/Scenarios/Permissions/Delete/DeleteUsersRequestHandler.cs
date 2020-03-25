using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Permissions.Delete;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Permissions.Delete
{
    public class DeleteUsersRequestHandler
        : IRequestHandler<DeletePermissionRequest>

    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DeleteUsersRequestHandler> _logger;

        public DeleteUsersRequestHandler(StoreDbContext storeDbContext, ILogger<DeleteUsersRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeletePermissionRequest request, CancellationToken cancellationToken)
        {
            var userPermission = await _storeDbContext
                .Permissions
                .Where(p => p.SubjectId == request.SubjectId)
                .SingleOrDefaultAsync(cancellationToken);

            if (userPermission != null)
            {
                _storeDbContext.Permissions
                    .Remove(userPermission);

                await _storeDbContext.SaveChangesAsync(cancellationToken);
            }

            Log.SubjectIdDoesNotExist(_logger, request.SubjectId);
            throw new InvalidOperationException("User permissions does not exist in the store.");
        }
    }
}
