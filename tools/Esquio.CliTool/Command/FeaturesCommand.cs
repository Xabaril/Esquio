using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
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
            console.WriteLine(Constants.SpecifySubCommandErrorMessage);

            app.ShowHelp(usePager:true);
            return 1;
        }

        private class RolloutCommand
        {
            [Option("--feature-id <FEATURE-ID>", Description = "The feature identifier to be rolled out.")]
            [Required]
            public int FeatureId { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; }

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; }


            private async Task<int> OnExecute(IConsole console)
            {
                var defaultForegroundColor = console.ForegroundColor;

                using (var client = EsquioClient.Create(
                    uri: Uri ?? Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable) ?? Constants.UriDefaultValue,
                    apikey: ApiKey ?? Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable)))
                {
                    var response = await client.RolloutFeatureAsync(FeatureId);

                    if (response.IsSuccessStatusCode)
                    {
                        console.ForegroundColor = Constants.SuccessColor;
                        console.WriteLine($"The feature with Id {FeatureId} was rolled out.");
                        console.ForegroundColor = defaultForegroundColor;

                        return 0;
                    }
                    else
                    {
                        console.ForegroundColor = Constants.ErrorColor;
                        console.WriteLine(await response.GetErrorDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 1;
                    }
                }
            }
        }

        private class RolloffCommand
        {
            [Option("--feature-id <FEATURE-ID>", Description = "The feature identifier to be rolled of.")]
            [Required]
            public int FeatureId { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; }

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; }

            private async Task<int> OnExecute(IConsole console)
            {
                var defaultForegroundColor = console.ForegroundColor;

                using (var client = EsquioClient.Create(
                    uri: Uri ?? Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable) ?? Constants.UriDefaultValue,
                    apikey: ApiKey ?? Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable)))
                {
                    var response = await client.RollbackFeatureAsync(FeatureId);

                    if (response.IsSuccessStatusCode)
                    {
                        console.ForegroundColor = Constants.SuccessColor;
                        console.WriteLine($"The feature with Id {FeatureId} was rolled out.");
                        console.ForegroundColor = defaultForegroundColor;

                        return 0;
                    }
                    else
                    {
                        console.ForegroundColor = Constants.ErrorColor;
                        console.WriteLine(await response.GetErrorDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 1;
                    }
                }
            }
        }

        private class ListCommand
        {
            [Option("--product-id <PRODUCT-ID>", Description = "The product id to list features.")]
            [Required]
            public int ProductId { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; }

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; }

            private async Task<int> OnExecute(IConsole console)
            {
                var defaultForegroundColor = console.ForegroundColor;

                using (var client = EsquioClient.Create(
                    uri: Uri ?? Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable) ?? Constants.UriDefaultValue,
                    apikey: ApiKey ?? Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable)))
                {
                    var response = await client.ListFeaturesAsync(ProductId);

                    if (response.IsSuccessStatusCode)
                    {
                        console.ForegroundColor = Constants.SuccessColor;
                        console.WriteLine(await response.GetContentDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 0;
                    }
                    else
                    {
                        console.ForegroundColor = Constants.ErrorColor;
                        console.WriteLine(await response.GetErrorDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 1;
                    }
                }
            }
        }
    }
}
