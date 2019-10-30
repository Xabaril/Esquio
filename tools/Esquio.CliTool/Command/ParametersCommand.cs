using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
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
            console.WriteLine(Constants.SpecifySubCommandErrorMessage);

            app.ShowHelp(usePager: true);
            return 1;
        }

        private class SetCommand
        {
            [Option("--toggle-id", Description = "The parameter identifier.")]
            [Required]
            public int ToggleId { get; set; }

            [Option("--name", Description = "The parameter value.")]
            [Required]
            public string ParameterName { get; set; }

            [Option("--value", Description = "The parameter value.")]
            [Required]
            public string ParameterValue { get; set; }

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
