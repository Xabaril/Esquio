using MediatR;
using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Toggles.Post
{
    public class PostToggleRequest : IRequest<int>
    {
        public int FeatureId { get; set; }

        public string ToggleType { get; set; }

        public Dictionary<string,string> Parameters { get; set; }
    }
}
