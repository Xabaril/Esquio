using MediatR;

namespace Esquio.UI.Api.Features.Tags.Delete
{
    public class DeleteTagRequest : IRequest
    {
        public string Tag { get; set; }
        public int FeatureId { get; set; }
    }
}
