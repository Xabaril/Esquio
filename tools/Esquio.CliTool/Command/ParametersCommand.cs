using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Esquio.CliTool.Command
{
    [Command(Constants.ParametersCommandName, Description = Constants.ParametersDescriptionCommandName),
        Subcommand(typeof(SetCommand))]
    internal class ParametersCommand
    {
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            app.ShowHelp(usePager: true);
            
            return 1;
        }

        [Command("set")]
        private class SetCommand
        {
            [Option("--product-name <PRODUCT-NAME>", Description = "The product name.")]
            [Required]
            public string ProductName { get; set; }

            [Option("--feature-name <FEATURE-NAME>", Description = "The feature name to be rolled off.")]
            [Required]
            public string FeatureName { get; set; }

            [Option("--deployment-name <DEPLOYMENT-NAME>", Description = "The deployment name to configure this value")]
            [Required]
            public string DeploymentName { get; set; }

            [Option("--toggle-type <TOGGLE-TYPE>", Description = "The toggle type.")]
            [Required]
            public string ToggleType { get; set; }

            [Option("--name <NAME>", Description = "The parameter value.")]
            [Required]
            public string ParameterName { get; set; }

            [Option("--value <VALUE>", Description = "The parameter value.")]
            [Required]
            public string ParameterValue { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; } = Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable);

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; } = Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable);

            private async Task<int> OnExecute(IConsole console)
            {
                var client = EsquioClientFactory.Instance
                    .Create(Uri, ApiKey);

                await client.Toggles_AddParameterAsync(
                    new AddParameterToggleRequest 
                    { 
                        ProductName = ProductName,
                        FeatureName = FeatureName,
                        ToggleType = ToggleType,
                        Name = ParameterName,
                        Value = ParameterValue,
                        DeploymentName = DeploymentName
                    });

                console.WriteLine($"The parameter {ParameterName} with value {ParameterValue} was added or updated succesfully.", Constants.SuccessColor);

                return 0;
            }
        }
    }
}
