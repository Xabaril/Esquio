using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;

namespace UnitTests.Seedwork
{
    internal class FakeHostEnvironment
        : IHostEnvironment
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }

        public FakeHostEnvironment(string environmentName, string applicationName = null, string contentRootPath = null, IFileProvider provider = null)
        {
            EnvironmentName = environmentName;
            ApplicationName = applicationName;
            ContentRootPath = contentRootPath;
            ContentRootFileProvider = provider;
        }
    }
}
