using Esquio.CliTool.Command;
using McMaster.Extensions.CommandLineUtils;

namespace Esquio.CliTool
{
    [Subcommand(typeof(ProductsCommand), typeof(FeaturesCommand))]
    [Command(Name = "dotnet-esquio")]
    class Program
    {
        static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine("You must specify at a subcommand.");
            app.ShowHelp();
            return 1;
        }
    }
}
