using Esquio.UI.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Esquio.UI.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Warning()
               .WriteTo.BrowserConsole()
               .CreateLogger();

            try
            {
                await ConfigureHost(args).Build().RunAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An exception is throwed when creating the WASM host!");
            }
        }

        static WebAssemblyHostBuilder ConfigureHost(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();
            builder.Services.AddOidcAuthentication(options =>
            {
                options.ProviderOptions.Authority = "https://demo.identityserver.io";
                options.ProviderOptions.ClientId = "interactive.public";
                options.ProviderOptions.DefaultScopes.Add("api");
                options.ProviderOptions.ResponseType = "code";
            });

            builder.Services.AddTransient<IEsquioHttpClient, EsquioHttpClient>(sp =>
            {
                var navigationManager = sp.GetRequiredService<NavigationManager>();
                var tokenService = sp.GetRequiredService<IAccessTokenProvider>();

                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(navigationManager.BaseUri);

                return new EsquioHttpClient(httpClient, tokenService);
            });

            builder.Services.AddScoped<EsquioState>();
            builder.Services.AddScoped<ILocalStorage, LocalStorage>();
            builder.Services.AddScoped<IPolicyBuilder, DefaultPolicyBuilder>();

            return builder;
        }
    }
}
