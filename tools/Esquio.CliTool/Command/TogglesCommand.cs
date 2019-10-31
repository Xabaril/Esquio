using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
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
            console.WriteLine(Constants.SpecifySubCommandErrorMessage);

            app.ShowHelp(usePager: true);
            return 1;
        }

        private class ListCommand
        {
            [Option("--feature-id <FEATURE-ID>", Description = "The feature identifier to list toggles.")]
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
                    var response = await client.ListTogglesAsync(FeatureId);

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

        private class GetCommand
        {
            [Option("--toggle-id <TOGGLE-ID>", Description = "The toggle identifier.")]
            [Required]
            public int ToggleId { get; set; }

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
                    var response = await client.GetToggleAsync(ToggleId);

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
