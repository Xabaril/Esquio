using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Infrastructure.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Users.Details
{
    public class DetailsUserRequestHandler
        : IRequestHandler<DetailsUsersRequest, DetailsUsersResponse>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DetailsUserRequestHandler> _logger;

        public DetailsUserRequestHandler(StoreDbContext storeDbContext,ILogger<DetailsUserRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DetailsUsersResponse> Handle(DetailsUsersRequest request, CancellationToken cancellationToken)
        {
            var userPermission = await _storeDbContext
               .Permissions
               .Where(p => p.SubjectId == request.SubjectId)
               .SingleOrDefaultAsync(cancellationToken);

            if (userPermission != null)
            {
                return new DetailsUsersResponse()
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
