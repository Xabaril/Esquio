using System;

namespace Esquio.UI.Api.Shared.Infrastructure.Data.Entities
{
    [Flags]
    public enum ApplicationRole
        : short
    {
        Reader = 1,
        Contributor = 2,
        Management = 3
    }
}
