using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Esquio.CliTool.Command
{
    [Command(Constants.ProductsCommandName, Description = Constants.ProductsDescriptionCommandName),
        Subcommand(typeof(AddCommand)),
        Subcommand(typeof(RemoveCommand)),
        Subcommand(typeof(ListCommand))]
    internal class ProductsCommand
    {
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            app.ShowHelp(usePager: true);

            return 1;
        }

        private class AddCommand
        {
            [Option("--name <NAME>", Description = "The name of product to be added.")]
            [Required]
            public string Name { get; set; }

            [Option("--description <DESCRIPTION>", Description = "The description of product to be added.")]
            [Required]
            public string Description { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; } = Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable);

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; } = Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable);

            private async Task<int> OnExecute(IConsole console)
            {
                var client = EsquioClientFactory.Instance
                    .Create(Uri, ApiKey);

                await client.Products_AddAsync(
                    new AddProductRequest
                    {
                        Name = Name,
                        Description = Description
                    });

                console.WriteLine($"The product {Name} was added succesfully.", Constants.SuccessColor);

                return 0;
            }
        }

        private class RemoveCommand
        {
            [Option("--name <NAME>", Description = "The name of product to delete.")]
            [Required]
            public string Name { get; set; }

            [Option(Constants.NoPromptParameter, Description = Constants.NoPromptDescription)]
            public bool NoPrompt { get; set; } = false;

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; } = Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable);

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; } = Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable);

            private async Task<int> OnExecute(IConsole console)
            {
                if (!NoPrompt)
                {
                    var proceed = Prompt.GetYesNo(
                        prompt: Constants.NoPromptMessage,
                        defaultAnswer: true,
                        promptColor: Constants.PromptColor,
                        promptBgColor: Constants.PromptBgColor);

                    if (!proceed)
                    {
                        return 0;
                    }
                }

                var client = EsquioClientFactory.Instance
                    .Create(Uri, ApiKey);

                await client.Products_DeleteAsync(Name);
                    
                console.WriteLine($"The product {Name} was removed succesfully.", Constants.SuccessColor);

                return 0;
            }
        }

        private class ListCommand
        {
            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; } = Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable);

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; } = Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable);

            [Option(Constants.PageIndexParameter, Description = Constants.PageIndexDescription)]
            public int PageIndex { get; set; } = Constants.PageIndex;

            [Option(Constants.PageCountParameter, Description = Constants.PageCountDescription)]
            public int PageCount { get; set; } = Constants.PageCount;

            private async Task<int> OnExecute(IConsole console)
            {
                var client = EsquioClientFactory.Instance
                    .Create(Uri, ApiKey);

                var response = await client.Products_ListAsync(pageIndex: PageIndex, pageCount: PageCount);

                console.WriteObject(response, Constants.SuccessColor);

                return 0;
            }
        }
    }
}
