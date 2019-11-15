using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace Esquio.CliTool.Command
{
    [Command(Constants.ParametersCommandName, Description = Constants.ParametersDescriptionCommandName),
        Subcommand(typeof(SetCommand))]
    internal class ParametersCommand
    {
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine(Constants.AsciiArt);
            console.WriteLine(Constants.SpecifySubCommandErrorMessage);

            app.ShowHelp(usePager: true);
            return 1;
        }

        private class SetCommand
        {
            [Option("--toggle-id <TOGGLE-ID>", Description = "The parameter identifier.")]
            [Required]
            public int ToggleId { get; set; }

            [Option("--name <NAME>", Description = "The parameter value.")]
            [Required]
            public string ParameterName { get; set; }

            [Option("--value <VALUE>", Description = "The parameter value.")]
            public string ParameterValue { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; } = Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable);

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; } = Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable);

            private async Task<int> OnExecute(IConsole console)
            {
                var defaultForegroundColor = console.ForegroundColor;

                using (var client = EsquioClient.Create(
                    uri: Uri ?? Constants.UriDefaultValue,
                    apikey: ApiKey))
                {
                    var response = await client.SetParameterValue(ToggleId, ParameterName, ParameterValue);

                    if (response.IsSuccessStatusCode)
                    {
                        console.ForegroundColor = Constants.SuccessColor;
                        console.WriteLine($"The parameter {ParameterName} with value {ParameterValue} was added or updated succesfully.");
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
