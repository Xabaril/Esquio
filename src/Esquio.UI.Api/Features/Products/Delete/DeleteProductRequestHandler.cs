using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Products.Delete
{
    public class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequest>
    {
        public Task<Unit> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
