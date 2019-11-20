using MediatR;

namespace Esquio.UI.Api.Features.Flags.List
{
    public class ListFeatureRequest : IRequest<ListFeatureResponse>
    {
        public int PageIndex { get; set; } = 0;

        public int PageCount { get; set; } = 10;

        internal string ProductName { get; set; }
    }
}
