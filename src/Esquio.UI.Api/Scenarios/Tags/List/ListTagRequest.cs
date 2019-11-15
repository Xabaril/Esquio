using MediatR;
using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Tags.List
{
    public class ListTagRequest : IRequest<List<TagResponseDetail>>
    {
        public string ProductName { get; set; }

        public string FeatureName { get; set; }
    }
}
