using McMaster.Extensions.CommandLineUtils;

namespace Esquio.CliTool
{
    [Subcommand(typeof(Products), typeof(Features))]
    [Command(Name = "dotnet-esquio")]
    class Program
    {
        static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Option("--uri", Description = "The Esquio UI url base path.")]
        public string Uri { get; set; }

        [Option("--apikey", Description = "The valid Esquio UI api key.")]
        public string ApiKey { get; set; }

        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine("You must specify at a subcommand.");
            app.ShowHelp();
            return 1;
        }

        [Command("products", Description = "Manage Esquio Products"),
         Subcommand(typeof(Add)),
         Subcommand(typeof(Remove))]
        private class Products
        {
            private int OnExecute(CommandLineApplication app, IConsole console)
            {
                console.WriteLine("You must specify the sub command.");

                app.ShowHelp();
                return 1;
            }

            private class Add
            {
                [Option("--name", Description = "The product name to be added.")]
                public string Name { get; set; }

                [Option("--description", Description = "The product description to be added")]
                public string Description { get; set; }

                private void OnExecute(IConsole console)
                {
                    console.WriteLine($"we are on Add product command {Name} {Description}");
                }
            }

            private class Remove
            {
                [Option("--name", Description = "The product name to delete.")]
                public string Name { get; set; }

                private void OnExecute(IConsole console)
                {
                    console.WriteLine($"we are on Remove product command {Name}");
                }
            }
        }

        [Command("features", Description = "Manage Esquio Features"),
         Subcommand(typeof(Rollout)),
         Subcommand(typeof(Rolloff))]
        private class Features
        {
            private int OnExecute(CommandLineApplication app, IConsole console)
            {
                console.WriteLine("You must specify the sub command.");

                app.ShowHelp();
                return 1;
            }

            private class Rollout
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

            private class Rolloff
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
}
