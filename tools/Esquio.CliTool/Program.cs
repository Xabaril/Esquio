using Esquio.CliTool.Command;
using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;

namespace Esquio.CliTool
{
    [Command(Name = Constants.CommandName)]
    [Subcommand(typeof(ProductsCommand),
        typeof(FeaturesCommand),
        typeof(TogglesCommand),
        typeof(ParametersCommand))]
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var app = new CommandLineApplication<Program>
                {
                    HelpTextGenerator = new EsquioHelpGenerator()
                };

                app.Conventions.UseDefaultConventions();

                return app.Execute(args);
            }
            catch (ApiException<ValidationProblemDetails> exception)
            {
                PhysicalConsole.Singleton.WriteObject(exception.Result, Constants.ErrorColor);
                return 1;
            }
            catch (ApiException<ProblemDetails> exception)
            {
                PhysicalConsole.Singleton.WriteObject(exception.Result, Constants.ErrorColor);
                return 1;
            }
        }

        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            app.ShowHelp(usePager: true);

            return 1;
        }
    }
}
