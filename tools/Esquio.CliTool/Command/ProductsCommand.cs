using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace Esquio.CliTool.Command
{
    [Command("products", Description = "Manage Esquio Products"),
        Subcommand(typeof(AddCommand)),
        Subcommand(typeof(RemoveCommand)),
        Subcommand(typeof(ListCommand))]
    internal class ProductsCommand
    {
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine("You must specify the sub command.");

            app.ShowHelp();
            return 1;
        }

        private class AddCommand
        {
            [Option("--name", Description = "The product name to be added.")]
            [Required]
            public string Name { get; set; }

            [Option("--description", Description = "The product description to be added")]
            [Required]
            public string Description { get; set; }

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
                    var response = await client.AddProductAsync(Name, Description);

                    if (response.IsSuccessStatusCode)
                    {
                        console.ForegroundColor = System.ConsoleColor.Green;
                        console.WriteLine($"The product {Name} was added succesfully.");
                        console.ForegroundColor = defaultForegroundColor;

                        return 0;
                    }
                    else
                    {
                        console.ForegroundColor = System.ConsoleColor.Red;
                        console.WriteLine(await response.GetErrorDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 1;
                    }
                }
            }
        }

        private class RemoveCommand
        {
            [Option("--id", Description = "The product id to delete.")]
            public int Id { get; set; }

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
                    var response = await client.RemoveProductAsync(Id);

                    if (response.IsSuccessStatusCode)
                    {
                        console.ForegroundColor = System.ConsoleColor.Green;
                        console.WriteLine($"The product with identifier {Id} was removed succesfully.");
                        console.ForegroundColor = defaultForegroundColor;

                        return 0;
                    }
                    else
                    {
                        console.ForegroundColor = System.ConsoleColor.Red;
                        console.WriteLine(await response.GetErrorDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 1;
                    }
                }
            }
        }

        private class ListCommand
        {
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
                    var response = await client.ListProductsAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        console.ForegroundColor = System.ConsoleColor.Green;
                        console.WriteLine(await response.GetContentDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 0;
                    }
                    else
                    {
                        console.ForegroundColor = System.ConsoleColor.Red;
                        console.WriteLine(await response.GetErrorDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 1;
                    }
                }
            }
        }
    }
}
