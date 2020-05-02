using MediatR;

namespace Esquio.UI.Api.Shared.Models.Tags.Delete
{
    public class DeleteTagRequest : IRequest
    {
        public string Tag { get; set; }

        public string FeatureName { get; set; }

        public string ProductName { get; set; }
    }
}
