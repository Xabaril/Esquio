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
        Subcommand(typeof(ArchiveCommand)),
        Subcommand(typeof(ListCommand))]
    internal class FeaturesCommand
    {
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            app.ShowHelp(usePager: true);

            return 1;
        }

        [Command("rollout")]
        private class RolloutCommand
        {
            [Option("--product-name <PRODUCT-NAME>", Description = "The product name.")]
            [Required]
            public string ProductName { get; set; }

            [Option("--feature-name <FEATURE-NAME>", Description = "The feature name to be rolled out.")]
            [Required]
            public string FeatureName { get; set; }

            [Option("--deployment-name <DEPLOYMENT-NAME>", Description = "The deployment name on where this feature is rolled out.")]
            [Required]
            public string DeploymentName { get; set; }

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

                await client.Features_RolloutAsync(ProductName, DeploymentName, FeatureName);

                console.WriteLine($"The feature {FeatureName} was rolled out.", Constants.SuccessColor);

                return 0;
            }
        }

        [Command("rolloff")]
        private class RolloffCommand
        {
            [Option("--product-name <PRODUCT-NAME>", Description = "The product name.")]
            [Required]
            public string ProductName { get; set; }

            [Option("--feature-name <FEATURE-NAME>", Description = "The feature name to be rolled off.")]
            [Required]
            public string FeatureName { get; set; }

            [Option("--deployment-name <DEPLOYMENT-NAME>", Description = "The deployment name on where the feature is rolled off.")]
            [Required]
            public string DeploymentName { get; set; }

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

                await client.Features_RollbackAsync(ProductName, DeploymentName, FeatureName);

                console.WriteLine($"The feature {FeatureName} was rolled off.", Constants.SuccessColor);

                return 0;
            }
        }

        [Command("archive")]
        private class ArchiveCommand
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

                await client.Features_ArchiveAsync(ProductName, FeatureName);

                console.WriteLine($"The feature {FeatureName} was archived succesfully.", Constants.SuccessColor);

                return 0;
            }
        }

        [Command("list")]
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
