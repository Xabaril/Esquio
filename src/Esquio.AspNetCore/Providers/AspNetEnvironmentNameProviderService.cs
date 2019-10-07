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
        private Task<string> _environmentName;

        public AspNetEnvironmentNameProviderService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        public Task<string> GetEnvironmentNameAsync()
        {
            if (_environmentName == null)
            {
                _environmentName = Task.FromResult(_hostingEnvironment.EnvironmentName);
            }

            return _environmentName;
        }
    }
}
