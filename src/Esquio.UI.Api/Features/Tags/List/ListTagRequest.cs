using MediatR;
using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Tags.List
{
    public class ListTagRequest : IRequest<List<TagResponseDetail>>
    {
        public int FeatureId { get; set; }
    }
}
