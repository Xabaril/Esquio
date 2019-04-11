using Esquio.Abstractions.Providers;
using System;
using System.Threading.Tasks;

namespace UnitTests.Seedwork
{
    class DelegatedUserNameProviderService
        : IUserNameProviderService
    {
        Func<string> _getUserNameFunc;

        public DelegatedUserNameProviderService(Func<string> getUserNameFunc)
        {
            _getUserNameFunc = getUserNameFunc;
        }
        public Task<string> GetCurrentUserNameAsync()
        {
            return Task.FromResult(_getUserNameFunc());
        }
    }
}
