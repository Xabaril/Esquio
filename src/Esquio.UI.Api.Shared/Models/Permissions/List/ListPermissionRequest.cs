using MediatR;

namespace Esquio.UI.Api.Shared.Models.Permissions.List
{
    public class ListPermissionRequest
        :IRequest<PaginatedResult<ListUsersResponseDetail>>
    {
        public int PageIndex { get; set; } = Constants.Pagination.PageIndex;

        public int PageCount { get; set; } = Constants.Pagination.PageIndex;
    }
}
