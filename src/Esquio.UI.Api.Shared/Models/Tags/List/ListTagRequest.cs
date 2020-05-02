using MediatR;
using System.Collections.Generic;

namespace Esquio.UI.Api.Shared.Models.Tags.List
{
    public class ListTagRequest : IRequest<IEnumerable<TagResponseDetail>>
    {
        public string ProductName { get; set; }

        public string FeatureName { get; set; }
    }
}
