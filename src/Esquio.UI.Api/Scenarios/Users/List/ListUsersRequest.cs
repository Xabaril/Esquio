using MediatR;

namespace Esquio.UI.Api.Features.Users.List
{
    public class ListUsersRequest
        :IRequest<ListUsersResponse>
    {
        public int PageIndex { get; set; } = 0;

        public int PageCount { get; set; } = 10;
    }
}
