using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Esquio.CliTool.Command
{
    [Command(Constants.FeaturesCommandName, Description = Constants.FeaturesDescriptionCommandName),
        Subcommand(typeof(RolloutCommand)),
        Subcommand(typeof(RolloffCommand)),
        Subcommand(typeof(ListCommand))]
    internal class FeaturesCommand
    {
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            app.ShowHelp(usePager: true);

            return 1;
        }

        private class RolloutCommand
        {
            [Option("--product-name <PRODUCT-NAME>", Description = "The product name.")]
            [Required]
            public string ProductName { get; set; }

            [Option("--feature-name <FEATURE-NAME>", Description = "The feature name to be rolled out.")]
            [Required]
            public string FeatureName { get; set; }

            [Option(Constants.NoPromptParameter, Description = Constants.NoPromptDescription)]
            public bool NoPrompt { get; set; } = false;

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; } = Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable);

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; } = Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable);

            private async Task<int> OnExecute(IConsole console)
            {
                if (!NoPrompt)
                {
                    var proceed = Prompt.GetYesNo(
                        prompt: Constants.NoPromptMessage,
                        defaultAnswer: true,
                        promptColor: Constants.PromptColor,
                        promptBgColor: Constants.PromptBgColor);

                    if (!proceed)
                    {
                        return 0;
                    }
                }

                var client = EsquioClientFactory.Instance
                    .Create(Uri, ApiKey);

                await client.Features_RolloutAsync(ProductName, FeatureName);

                console.WriteLine($"The feature {FeatureName} was rolled out.", Constants.SuccessColor);

                return 0;
            }
        }

        private class RolloffCommand
        {
            [Option("--product-name <PRODUCT-NAME>", Description = "The product name.")]
            [Required]
            public string ProductName { get; set; }

            [Option("--feature-name <FEATURE-NAME>", Description = "The feature name to be rolled off.")]
            [Required]
            public string FeatureName { get; set; }

            [Option(Constants.NoPromptParameter, Description = Constants.NoPromptDescription)]
            public bool NoPrompt { get; set; } = false;

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; } = Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable);

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; } = Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable);

            private async Task<int> OnExecute(IConsole console)
            {
                if (!NoPrompt)
                {
                    var proceed = Prompt.GetYesNo(
                        prompt: Constants.NoPromptMessage,
                        defaultAnswer: true,
                        promptColor: Constants.PromptColor,
                        promptBgColor: Constants.PromptBgColor);

                    if (!proceed)
                    {
                        return 0;
                    }
                }

                var client = EsquioClientFactory.Instance
                    .Create(Uri, ApiKey);

                await client.Features_RollbackAsync(ProductName, FeatureName);

                console.WriteLine($"The feature {FeatureName} was rolled off.", Constants.SuccessColor);

                return 0;
            }
        }

        private class ListCommand
        {
            [Option("--product-name <PRODUCT-NAME>", Description = "The product to list features.")]
            [Required]
            public string ProductName { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; } = Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable);

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; } = Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable);

            [Option(Constants.PageIndexParameter, Description = Constants.PageIndexDescription)]
            public int PageIndex { get; set; } = Constants.PageIndex;

            [Option(Constants.PageCountParameter, Description = Constants.PageCountDescription)]
            public int PageCount { get; set; } = Constants.PageCount;

            private async Task<int> OnExecute(IConsole console)
            {
                var client = EsquioClientFactory.Instance
                    .Create(Uri, ApiKey);

                var response = await client.Features_ListAsync(ProductName, PageIndex, PageCount);
                console.WriteObject(response, Constants.SuccessColor);

                return 0;
            }
        }
    }
}
