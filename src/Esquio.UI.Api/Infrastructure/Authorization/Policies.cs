using System;

namespace Esquio.UI.Api.Infrastructure.Authorization
{
    static class Policies
    {
        public const string Read = nameof(Read);
        public const string Write = nameof(Write);
        public const string Management = nameof(Management);
    }
}
