using MediatR;

namespace Esquio.UI.Api.Shared.Models.Users.List
{
    public class ListUsersRequest
        :IRequest<PaginatedResult<ListUsersResponseDetail>>
    {
        public int PageIndex { get; set; } = Constants.Pagination.PageIndex;

        public int PageCount { get; set; } = Constants.Pagination.PageIndex;
    }
}
