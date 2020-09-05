using Esquio.UI.Api.Shared.Settings;
using Esquio.UI.Client.Infrastructure.Authorization;
using Esquio.UI.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Esquio.UI.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //await ConfigureHost(args).Build().RunAsync();
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents
                .Add<App>("app");

            builder.Logging
                .SetMinimumLevel(LogLevel.Warning);

            builder.Services.AddHttpClient<IEsquioHttpClient, EsquioHttpClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            var isAzureAd = builder.Configuration.GetValue<bool>("Security:IsAzureAd");
            var authority = builder.Configuration.GetValue<string>("Security:Authority");
            var clientId = builder.Configuration.GetValue<string>("Security:ClientId");
            var scope = builder.Configuration.GetValue<string>("Security:Scope");
            var responseType = builder.Configuration.GetValue<string>("Security:ResponseType");

            if (isAzureAd)
            {
                builder.Services.AddMsalAuthentication(options =>
                {
                    options.ProviderOptions.Authentication.Authority = authority;
                    options.ProviderOptions.Authentication.ClientId = clientId;
                    options.ProviderOptions.DefaultAccessTokenScopes.Add(scope);
                    options.ProviderOptions.Authentication.ValidateAuthority = false;
                    options.ProviderOptions.Authentication.NavigateToLoginRequestUrl = true;
                });
            }
            else
            {
                builder.Services.AddOidcAuthentication(options =>
                {
                    options.ProviderOptions.Authority = authority;
                    options.ProviderOptions.ClientId = clientId;
                    options.ProviderOptions.ResponseType = responseType;
                    options.ProviderOptions.DefaultScopes.Add(scope);
                });
            }

           
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


            var host =  builder.Build();

            var jsRuntime = builder.Services.BuildServiceProvider().GetRequiredService<IJSRuntime>();
            await jsRuntime.InvokeVoidAsync("addAuthenticationScript", new object[] { isAzureAd });

            await host.RunAsync();

        }

        //static WebAssemblyHostBuilder ConfigureHost(string[] args)
        //{
        //    var builder = WebAssemblyHostBuilder.CreateDefault(args);

        //    builder.RootComponents
        //        .Add<App>("app");

        //    builder.Logging
        //        .SetMinimumLevel(LogLevel.Warning);

        //    builder.Services.AddHttpClient<IEsquioHttpClient, EsquioHttpClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
        //        .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        //    var isAzureAd = builder.Configuration.GetValue<bool>("Security:IsAzureAd");
        //    var authority = builder.Configuration.GetValue<string>("Security:Authority");
        //    var clientId = builder.Configuration.GetValue<string>("Security:ClientId");
        //    var scope = builder.Configuration.GetValue<string>("Security:Scope");
        //    var responseType = builder.Configuration.GetValue<string>("Security:ResponseType");

        //    if (isAzureAd)
        //    {
        //        builder.Services.AddMsalAuthentication(options =>
        //        {
        //            options.ProviderOptions.Authentication.Authority = authority;
        //            options.ProviderOptions.Authentication.ClientId = clientId;
        //            options.ProviderOptions.DefaultAccessTokenScopes.Add(scope);
        //        });
        //    }
        //    else
        //    {
        //        builder.Services.AddOidcAuthentication(options =>
        //        {
        //            options.ProviderOptions.Authority = authority;
        //            options.ProviderOptions.ClientId = clientId;
        //            options.ProviderOptions.ResponseType = responseType;
        //            options.ProviderOptions.DefaultScopes.Add(scope);
        //        });
        //    }

        //    var jsRuntime = builder.Services.BuildServiceProvider().GetRequiredService<IJSRuntime>();
        //    jsRuntime.InvokeVoidAsync("addAuthenticationScript", new object[] { isAzureAd });

        //    builder.Services.AddAuthorizationCore(options =>
        //    {
        //        options.AddPolicy(Policies.Reader, builder => builder.AddRequirements(new PolicyRequirement(Policies.Reader)));
        //        options.AddPolicy(Policies.Contributor, builder => builder.AddRequirements(new PolicyRequirement(Policies.Contributor)));
        //        options.AddPolicy(Policies.Management, builder => builder.AddRequirements(new PolicyRequirement(Policies.Management)));
        //    });

        //    builder.Services.AddScoped<EsquioState>();
        //    builder.Services.AddScoped<ConfirmState>();
        //    builder.Services.AddScoped<ILocalStorage, LocalStorage>();
        //    builder.Services.AddScoped<IPolicyBuilder, DefaultPolicyBuilder>();
        //    builder.Services.AddScoped<INotifications, Notifications>();
        //    builder.Services.AddScoped<IAuthorizationHandler, PolicyRequirementHandler>();


        //    return builder;
        //}
    }
}
