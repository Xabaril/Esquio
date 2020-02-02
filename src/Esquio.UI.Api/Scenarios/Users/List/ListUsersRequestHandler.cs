using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Users.List
{
    public class ListUsersRequestHandler
        : IRequestHandler<ListUsersRequest,ListUsersResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public ListUsersRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }

        public async Task<ListUsersResponse> Handle(ListUsersRequest request, CancellationToken cancellationToken = default)
        {
            var total = await _storeDbContext.Permissions
                .CountAsync(cancellationToken);

            var userPermissions = await _storeDbContext
                .Permissions
                .Skip(request.PageIndex * request.PageCount)
                .Take(request.PageCount)
                .Select(u => new ListUsersResponseDetail()
                {
                    SubjectId = u.SubjectId,
                    ManagementPermission = u.ManagementPermission,
                    ReadPermission = u.ReadPermission,
                    WritePermission = u.WritePermission
                }).ToListAsync(cancellationToken);

            return new ListUsersResponse()
            {
                Total = total,
                PageIndex = request.PageIndex,
                Count = userPermissions.Count,
                Result = userPermissions
            };
        }
    }
}
