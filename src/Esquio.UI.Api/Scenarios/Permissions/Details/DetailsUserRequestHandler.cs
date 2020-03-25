using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Shared.Models.Permissions.Details;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Permissions.Details
{
    public class DetailsUserRequestHandler
        : IRequestHandler<DetailsPermissionRequest, DetailsPermissionResponse>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DetailsUserRequestHandler> _logger;

        public DetailsUserRequestHandler(StoreDbContext storeDbContext,ILogger<DetailsUserRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DetailsPermissionResponse> Handle(DetailsPermissionRequest request, CancellationToken cancellationToken)
        {
            var userPermission = await _storeDbContext
               .Permissions
               .Where(p => p.SubjectId == request.SubjectId)
               .SingleOrDefaultAsync(cancellationToken);

            if (userPermission != null)
            {
                return new DetailsPermissionResponse()
                {
                    SubjectId = userPermission.SubjectId,
                    ActAs = Enum.GetName(typeof(ApplicationRole),userPermission.ApplicationRole)
                };
            }

            Log.SubjectIdDoesNotExist(_logger, request.SubjectId);
            throw new InvalidOperationException("User permissions does not exist in the store.");
        }
    }
}
