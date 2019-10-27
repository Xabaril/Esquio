using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esquio.CliTool.Command
{
    [Command("features", Description = "Manage Esquio Features"),
         Subcommand(typeof(RolloutCommand)),
         Subcommand(typeof(RolloffCommand))]
    internal class FeaturesCommand
    {
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine("You must specify the sub command.");

            app.ShowHelp();
            return 1;
        }

        private class RolloutCommand
        {
            [Option("--name", Description = "The feature name to be rolled out.")]
            public string Name { get; set; }

            [Option("--product", Description = "The product where feature is created.")]
            public string Product { get; set; }

            private void OnExecute(IConsole console)
            {
                console.WriteLine($"we are on Rollout feature command {Name} {Product}");
            }
        }

        private class RolloffCommand
        {
            [Option("--name", Description = "The feature name to be rolled out.")]
            public string Name { get; set; }

            [Option("--product", Description = "The product where the feature is created.")]
            public string Product { get; set; }

            private void OnExecute(IConsole console)
            {
                console.WriteLine($"we are on Remove product command {Name}");
            }
        }
    }
}
