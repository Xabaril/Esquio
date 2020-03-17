using Esquio.UI.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Esquio.UI.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //add services
            builder.Services.AddBaseAddressHttpClient();
            builder.Services.AddOidcAuthentication(options =>
            {
                options.ProviderOptions.Authority = "https://demo.identityserver.io";
                options.ProviderOptions.ClientId = "interactive.public";
                options.ProviderOptions.DefaultScopes.Add("api");
                options.ProviderOptions.ResponseType = "code";
            });

            builder.Services.AddTransient<IEsquioClient, EsquioClient>(sp =>
             {
                 var navigationManager = sp.GetRequiredService<NavigationManager>();
                 var tokenService = sp.GetRequiredService<IAccessTokenProvider>();

                 var httpClient = new HttpClient();
                 httpClient.BaseAddress = new Uri(navigationManager.BaseUri);

                 return new EsquioClient(new ApiConfiguration(httpClient, tokenService));
             });

            builder.Services.AddScoped<ILocalStorage, LocalStorage>();
            builder.Services.AddScoped<IPolicyBuilder, DefaultPolicyBuilder>();
            builder.Services.AddScoped<EsquioState>();

            await builder.Build().RunAsync();
        }
    }
}
