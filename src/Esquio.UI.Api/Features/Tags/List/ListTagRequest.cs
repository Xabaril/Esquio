using MediatR;
using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Tags.List
{
    public class ListTagRequest : IRequest<List<ListTagResponse>>
    {
        public int FeatureId { get; set; }
    }
}
