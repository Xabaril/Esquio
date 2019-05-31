using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Toggles.AddParameter
{
    public class AddParameterToggleRequestHandler : IRequestHandler<AddParameterToggleRequest>
    {
        private readonly StoreDbContext _storeDbContext;

        public AddParameterToggleRequestHandler(StoreDbContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            _storeDbContext = context;
        }

        public async Task<Unit> Handle(AddParameterToggleRequest request, CancellationToken cancellationToken)
        {
            var toggle = await _storeDbContext
                .Toggles
                .Include(t => t.Parameters)
                .Where(t => t.Id == request.ToggleId)
                .SingleOrDefaultAsync(cancellationToken);

            if (toggle != null)
            {
                var parameter = toggle.Parameters
                    .Where(p => p.Name.Equals(request.Name))
                    .SingleOrDefault();

                if (parameter != null)
                {
                    parameter.Value = request.Value;
                }
                else
                {
                    toggle.Parameters
                        .Add(new ParameterEntity(toggle.Id, request.Name, request.Value));
                }

                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            throw new InvalidOperationException($"Toggle with id {request.ToggleId} does not exist.");
        }
    }
}
