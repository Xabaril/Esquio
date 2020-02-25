using MediatR;
using System;

namespace Esquio.UI.Api.Scenarios.ApiKeys.Add
{
    public class AddApiKeyRequest : IRequest<AddApiKeyResponse>
    {
        public string Name { get; set; }

        public string ActAs { get; set; }

        public DateTime? ValidTo { get; set; }
    }
}
