using Esquio.CliTool.Command;
using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
using System.Net;

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

            try
            {
                return app.Execute(args);
            }
            catch (ApiException apiException) when (apiException.StatusCode == (int)HttpStatusCode.NotFound)
            {
                PhysicalConsole.Singleton.WriteLine(Constants.NotFoundErrorMessage, Constants.ErrorColor);
                return 1;
            }
            catch (ApiException apiException) when (apiException.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                PhysicalConsole.Singleton.WriteLine(Constants.UnauthorizedErrorMessage, Constants.ErrorColor);
                return 1;
            }
            catch (ApiException apiException) when (apiException.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                PhysicalConsole.Singleton.WriteLine(Constants.ForbiddenErrorMessage, Constants.ErrorColor);
                return 1;
            }
            catch (ApiException<ProblemDetails> apiException)
            {
                PhysicalConsole.Singleton.WriteLine(Constants.BadRquestErrorMessage, Constants.ErrorColor);
                PhysicalConsole.Singleton.WriteLine(apiException.Result.Detail, Constants.ErrorColor);
                return 1;
            }
        }

        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            app.ShowHelp(usePager: true);

            return 0;
        }
    }
}
