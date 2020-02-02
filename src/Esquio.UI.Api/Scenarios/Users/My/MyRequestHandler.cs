using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Users.My
{
    public class MyRequestHandler
        : IRequestHandler<MyRequest, MyResponse>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<MyRequestHandler> _logger;

        public MyRequestHandler(StoreDbContext storeDbContext,ILogger<MyRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<MyResponse> Handle(MyRequest request, CancellationToken cancellationToken)
        {
            var permission = await _storeDbContext.Permissions
                .Where(p => p.SubjectId == request.SubjectId)
                .SingleOrDefaultAsync(cancellationToken);

            if (permission != null)
            {
                return MyResponse.With(
                    readPermission: permission.ReadPermission,
                    writePermission: permission.WritePermission,
                    managementPermission: permission.ManagementPermission);
            }

            Log.MyIsNotAuthorized(_logger, request.SubjectId);
            return MyResponse.UnAuthorized();
        }
    }
}
