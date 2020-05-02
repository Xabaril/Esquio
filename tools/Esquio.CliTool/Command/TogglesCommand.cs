using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Esquio.CliTool.Command
{
    [Command(Constants.TogglesCommandName, Description = Constants.TogglesDescriptionCommandName),
        Subcommand(typeof(GetCommand)),
        Subcommand(typeof(ListCommand))]
    internal class TogglesCommand
    {
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            app.ShowHelp(usePager: true);

            return 1;
        }

        [Command("list")]
        private class ListCommand
        {
            [Option("--product-name <PRODUCT-NAME>", Description = "The product name.")]
            [Required]
            public string ProductName { get; set; }

            [Option("--feature-name <FEATURE-NAME>", Description = "The feature name to be rolled off.")]
            [Required]
            public string FeatureName { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; } = Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable);

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; } = Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable);

            private async Task<int> OnExecute(IConsole console)
            {
                var client = EsquioClientFactory.Instance
                    .Create(Uri, ApiKey);

                var response = await client.Features_GetAsync(ProductName, FeatureName);

                console.WriteObject(response, Constants.SuccessColor);

                return 0;
            }
        }

        [Command("get")]
        private class GetCommand
        {
            [Option("--product-name <PRODUCT-NAME>", Description = "The product name.")]
            [Required]
            public string ProductName { get; set; }

            [Option("--feature-name <FEATURE-NAME>", Description = "The feature name to be rolled off.")]
            [Required]
            public string FeatureName { get; set; }

            [Option("--toggle-type <TOGGLE-TYPE>", Description = "The toggle identifier.")]
            [Required]
            public string ToggleType { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; } = Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable);

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; } = Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable);

            private async Task<int> OnExecute(IConsole console)
            {
                var client = EsquioClientFactory.Instance
                    .Create(Uri, ApiKey);

                var response = await client.Toggles_DetailsAsync(ProductName, FeatureName, ToggleType);

                console.WriteObject(response, Constants.SuccessColor);

                return 0;
            }
        }
    }
}
