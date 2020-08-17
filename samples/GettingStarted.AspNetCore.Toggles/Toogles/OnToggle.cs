using Esquio.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GettingStarted.AspNetCore.Toggles
{
    public class OnToggle
        : IToggle
    {
        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            return new ValueTask<bool>(true);
        }
    }
}
