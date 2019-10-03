using Esquio.Abstractions.Providers;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Providers
{
    internal class AspNetEnvironmentNameProviderService
        : IEnvironmentNameProviderService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AspNetEnvironmentNameProviderService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        public Task<string> GetEnvironmentNameAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_hostingEnvironment.EnvironmentName);
        }
    }
}
