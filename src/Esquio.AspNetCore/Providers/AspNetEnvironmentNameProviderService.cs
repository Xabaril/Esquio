using Esquio.Abstractions.Providers;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Providers
{
    public class AspNetEnvironmentNameProviderService
        : IEnvironmentNameProviderService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public AspNetEnvironmentNameProviderService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }
        public Task<string> GetEnvironmentNameAsync()
        {
            return Task.FromResult(_hostingEnvironment.EnvironmentName);
        }
    }
}
