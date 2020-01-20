using MediatR;

namespace Esquio.UI.Api.Scenarios.Tags.Add
{
    public class AddTagRequest : IRequest
    {
        protected AddTagRequest() { }

        public AddTagRequest(string tag)
        {
            Tag = tag;
        }

        public string Tag { get; set; }

        internal string ProductName { get; set; }

        internal string FeatureName { get; set; }
    }
}
