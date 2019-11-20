using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;
using System.IO;

namespace Esquio.CliTool.Internal
{
    internal class EsquioHelpGenerator
        : DefaultHelpTextGenerator
    {
        protected override void GenerateHeader(CommandLineApplication application, TextWriter output)
        {
            output.WriteLine(Constants.AsciiArt);

            base.GenerateHeader(application, output);
        }
    }
}
