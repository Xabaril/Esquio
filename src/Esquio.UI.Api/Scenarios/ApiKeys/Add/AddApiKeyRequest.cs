using MediatR;
using System;

namespace Esquio.UI.Api.Scenarios.ApiKeys.Add
{
    public class AddApiKeyRequest : IRequest<AddApiKeyResponse>
    {
        public string Name { get; set; }

        public bool Read { get; set; }

        public bool Write { get; set; }

        public bool Management { get; set; }

        public DateTime? ValidTo { get; set; }
    }
}
