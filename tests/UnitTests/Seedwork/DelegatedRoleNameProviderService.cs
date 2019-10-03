using Esquio.Abstractions.Providers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Seedwork
{
    class DelegatedRoleNameProviderService
           : IRoleNameProviderService
    {
        Func<string> _getCurrentRoleFunc;
        public DelegatedRoleNameProviderService(Func<string> getCurrentRoleFunc)
        {
            _getCurrentRoleFunc = getCurrentRoleFunc ?? throw new ArgumentNullException(nameof(getCurrentRoleFunc));
        }
        public Task<string> GetCurrentRoleNameAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_getCurrentRoleFunc());
        }
    }
}
