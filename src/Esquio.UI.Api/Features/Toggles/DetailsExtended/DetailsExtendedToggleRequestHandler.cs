using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Toggles.DetailsExtended
{
    public class DetailsExtendedToggleRequestHandler : IRequestHandler<DetailsExtendedToggleRequest, DetailsExtendedToggleResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public DetailsExtendedToggleRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }

        public async Task<DetailsExtendedToggleResponse> Handle(DetailsExtendedToggleRequest request, CancellationToken cancellationToken)
        {
            var toggle = await _storeDbContext
                .Toggles
                .Include(t => t.Parameters)
                .Where(t => t.Id == request.ToggleId)
                .SingleOrDefaultAsync(cancellationToken);

            if (toggle != null)
            {
                return new DetailsExtendedToggleResponse()
                {
                    TypeName = toggle.Type,
                    Parameters = toggle.Parameters.Select(parameter => new ParameterDetail {
                        Id = parameter.Id,
                        Name = parameter.Name,
                        Value = parameter.Value
                    }).ToList()
                };
            }

            return null;
        }
    }
}
