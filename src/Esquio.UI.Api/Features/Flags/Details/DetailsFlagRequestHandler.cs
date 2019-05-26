using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Details
{
    public class DetailsFlagRequestHandler : IRequestHandler<DetailsFlagRequest, DetailsFlagResponse>
    {
        public Task<DetailsFlagResponse> Handle(DetailsFlagRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
