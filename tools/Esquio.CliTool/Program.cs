using Esquio.CliTool.Command;
using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Abstractions;

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
            var app = new CommandLineApplication<Program>
            {
                HelpTextGenerator = new EsquioHelpGenerator()
            };

            app.Conventions.UseDefaultConventions();

            return app.Execute(args);
        }
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            app.ShowHelp(usePager: true);

            return 1;
        }
    }
}
