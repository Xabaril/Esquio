using MediatR;
using System;

namespace Esquio.UI.Api.Features.ApiKeys.Add
{
    public class AddApiKeyRequest : IRequest<AddApiKeyResponse>
    {
        public string Name { get; set; }

        public DateTime? ValidTo { get; set; }
    }
}
