using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Esquio.UI.Api.Features.Tags.Add
{
    public class AddTagRequest : IRequest
    {
        protected AddTagRequest() { }

        public AddTagRequest(string tag)
        {
            Tag = tag;
        }

        public string Tag { get; set; }

        internal int FeatureId { get; set; }
    }
}
