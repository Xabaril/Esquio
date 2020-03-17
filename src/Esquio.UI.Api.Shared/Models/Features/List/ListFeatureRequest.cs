using MediatR;

namespace Esquio.UI.Api.Shared.Models.Features.List
{
    public class ListFeatureRequest : IRequest<PaginatedResult<ListFeatureResponseDetail>>
    {
        public int PageIndex { get; set; } = 0;

        public int PageCount { get; set; } = 10;

        internal string ProductName { get; set; }
    }
}
