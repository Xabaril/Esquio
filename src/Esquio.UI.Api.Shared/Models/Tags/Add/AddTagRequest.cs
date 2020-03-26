using MediatR;
using System;

namespace Esquio.UI.Api.Shared.Models.Tags.Add
{
    public class AddTagRequest : IRequest
    {
        protected AddTagRequest() { }

        public AddTagRequest(string tag,string hexColor = null)
        {
            Tag = tag ?? throw new ArgumentNullException(nameof(tag));
            HexColor = hexColor;
        }

        public string Tag { get; set; }

        public string HexColor { get; set; }

        internal string ProductName { get; set; }

        internal string FeatureName { get; set; }
    }
}
