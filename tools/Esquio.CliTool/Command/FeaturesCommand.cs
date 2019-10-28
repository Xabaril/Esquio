using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace Esquio.CliTool.Command
{
    [Command("features", Description = "Manage Esquio Features"),
        Subcommand(typeof(RolloutCommand)),
        Subcommand(typeof(RolloffCommand)),
        Subcommand(typeof(ListCommand))]
    internal class FeaturesCommand
    {
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine("You must specify the sub command.");

            app.ShowHelp();
            return 1;
        }

        private class RolloutCommand
        {
            [Option("--feature-id", Description = "The feature name to be rolled out.")]
            [Required]
            public int FeatureId { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            [Required]
            public string Uri { get; set; }

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            [Required]
            public string ApiKey { get; set; }


            private async Task<int> OnExecute(IConsole console)
            {
                var defaultForegroundColor = console.ForegroundColor;
                using (var client = EsquioClient.Create(Uri, ApiKey))
                {
                    var response = await client.RolloutFeatureAsync(FeatureId);

                    if (response.IsSuccessStatusCode)
                    {
                        console.ForegroundColor = System.ConsoleColor.Green;
                        console.WriteLine($"The feature with Id {FeatureId} was rolled out.");
                        console.ForegroundColor = defaultForegroundColor;

                        return 0;
                    }
                    else
                    {
                        console.ForegroundColor = System.ConsoleColor.Red;
                        console.WriteLine(await response.GetErrorDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 1;
                    }
                }
            }
        }

        private class RolloffCommand
        {
            [Option("--feature-id", Description = "The feature name to be rolled out.")]
            [Required]
            public int FeatureId { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            [Required]
            public string Uri { get; set; }

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            [Required]
            public string ApiKey { get; set; }

            private async Task<int> OnExecute(IConsole console)
            {
                var defaultForegroundColor = console.ForegroundColor;
                using (var client = EsquioClient.Create(Uri, ApiKey))
                {
                    var response = await client.RollbackFeatureAsync(FeatureId);

                    if (response.IsSuccessStatusCode)
                    {
                        console.ForegroundColor = System.ConsoleColor.Green;
                        console.WriteLine($"The feature with Id {FeatureId} was rolled out.");
                        console.ForegroundColor = defaultForegroundColor;

                        return 0;
                    }
                    else
                    {
                        console.ForegroundColor = System.ConsoleColor.Red;
                        console.WriteLine(await response.GetErrorDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 1;
                    }
                }
            }
        }

        private class ListCommand
        {
            [Option("--product-id", Description = "The feature name to be rolled out.")]
            [Required]
            public int ProductId { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            [Required]
            public string Uri { get; set; }

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            [Required]
            public string ApiKey { get; set; }

            private async Task<int> OnExecute(IConsole console)
            {
                var defaultForegroundColor = console.ForegroundColor;
                using (var client = EsquioClient.Create(Uri, ApiKey))
                {
                    var response = await client.ListFeaturesAsync(ProductId);

                    if (response.IsSuccessStatusCode)
                    {
                        console.ForegroundColor = System.ConsoleColor.Green;
                        console.WriteLine(await response.GetContentDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 0;
                    }
                    else
                    {
                        console.ForegroundColor = System.ConsoleColor.Red;
                        console.WriteLine(await response.GetErrorDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 1;
                    }
                }
            }
        }
    }
}
