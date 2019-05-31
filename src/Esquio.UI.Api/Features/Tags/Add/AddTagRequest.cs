using MediatR;

namespace Esquio.UI.Api.Features.Tags.Add
{
    public class AddTagRequest : IRequest
    {
        public AddTagRequest(string tag, int featureId)
        {
            Tag = tag;
            FeatureId = featureId;
        }

        public string Tag { get; set; }
        public int FeatureId { get; set; }
    }
}
