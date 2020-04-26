using Esquio.UI.Client.Infrastructure.Authorization;
using Esquio.UI.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
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

            builder.Services.AddHttpClient<IEsquioHttpClient,EsquioHttpClient>( client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Security", options.ProviderOptions);

                var audience = builder.Configuration
                    .GetValue<string>("Security:Audience");

                options.ProviderOptions.DefaultScopes.Add(audience);
            });

            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(Policies.Reader, builder => builder.AddRequirements(new PolicyRequirement(Policies.Reader)));
                options.AddPolicy(Policies.Contributor, builder => builder.AddRequirements(new PolicyRequirement(Policies.Contributor)));
                options.AddPolicy(Policies.Management, builder => builder.AddRequirements(new PolicyRequirement(Policies.Management)));
            });

            builder.Services.AddScoped<EsquioState>();
            builder.Services.AddScoped<ConfirmState>();
            builder.Services.AddScoped<ILocalStorage, LocalStorage>();
            builder.Services.AddScoped<IPolicyBuilder, DefaultPolicyBuilder>();
            builder.Services.AddScoped<INotifications, Notifications>();
            builder.Services.AddScoped<IAuthorizationHandler, PolicyRequirementHandler>();

            return builder;
        }
    }
}
