using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Toggles.Details
{
    public class DetailsToggleRequestHandler : IRequestHandler<DetailsToggleRequest, DetailsToggleResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public DetailsToggleRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }

        public async Task<DetailsToggleResponse> Handle(DetailsToggleRequest request, CancellationToken cancellationToken)
        {
            var toggle = await _storeDbContext
                .Toggles
                .Include(t => t.Parameters)
                .Where(t => t.Id == request.ToggleId)
                .SingleOrDefaultAsync(cancellationToken);

            if (toggle != null)
            {
                return new DetailsToggleResponse()
                {
                    TypeName = toggle.Type,
                    Parameters = toggle.Parameters.ToDictionary(p => p.Name, p => p.Value)
                };
            }

            return null;
        }
    }
}
