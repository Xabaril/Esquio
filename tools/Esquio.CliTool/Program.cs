using Esquio.CliTool.Command;
using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
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
            catch (ApiException exception)
            {
                var defaultForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = Constants.ErrorColor;
                Console.WriteLine(exception.Message);
                Console.ForegroundColor = defaultForegroundColor;

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
