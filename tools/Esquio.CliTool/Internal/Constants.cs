using System;

namespace Esquio.CliTool.Internal
{
    internal class Constants
    {
        public const string CommandName = "dotnet-esquio";

        public const string ProductsCommandName = "products";
        public const string ProductsDescriptionCommandName = "Manage Esquio products using Esquio UI HTTP API";


        public const string FeaturesCommandName = "features";
        public const string FeaturesDescriptionCommandName = "Manage Esquio features using Esquio UI HTTP API";

        public const string TogglesCommandName = "toggles";
        public const string TogglesDescriptionCommandName = "Manage Esquio toggles using Esquio UI HTTP API";

        public const string ParametersCommandName = "parameters";
        public const string ParametersDescriptionCommandName = "Manage Esquio parameters using Esquio UI HTTP API";

        public const string NoPromptParameter = "--no-prompt <NO-PROMPT>";
        public const string NoPromptDescription = "Show or hide prompt message";
        public const string NoPromptMessage = "Are you sure to perform this action?";
        

        public const string ApiKeyEnvironmentVariable = "ESQUIO_API_KEY";
        public const string ApiKeyParameter = "--api-key <APIKEY>";
        public const string ApiKeyDescription = "The valid Esquio UI Api Key used for Esquio authentication. If is not present, the environment variable ESQUIO_API_KEY will be used.";

        public const string UriEnvironmentVariable = "ESQUIO_URI";
        public const string UriDefaultValue = "http://localhost";
        public const string UriParameter = "--uri <URI>";
        public const string UriDescription = "The Esquio UI url base path.If is not present, the environment variable ESQUIO_URI will be used.";

        public const string SpecifySubCommandErrorMessage = "You must specify at a subcommand";

        public const ConsoleColor PromptColor = ConsoleColor.White;
        public const ConsoleColor PromptBgColor = ConsoleColor.Black;
        public const ConsoleColor ErrorColor = ConsoleColor.Red;
        public const ConsoleColor SuccessColor = ConsoleColor.Green;
    }
}
