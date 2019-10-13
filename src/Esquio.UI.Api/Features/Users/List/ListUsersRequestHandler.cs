using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Users.List
{
    public class ListUsersRequestHandler
        : IRequestHandler<ListUsersRequest,ListUsersResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public ListUsersRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }

        public async Task<ListUsersResponse> Handle(ListUsersRequest request, CancellationToken cancellationToken)
        {
            var userPermissions = await _storeDbContext
                .Permissions
                .Select(u => new ListUsersResponseDetail()
                {
                    SubjectId = u.SubjectId,
                    ManagementPermission = u.ManagementPermission,
                    ReadPermission = u.ReadPermission,
                    WritePermission = u.WritePermission
                }).ToListAsync();

            return new ListUsersResponse()
            {
                UserPermissions = userPermissions
            };
        }
    }
}
