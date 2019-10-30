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

        public const string ApiKeyParameter = "--api-key";
        public const string ApiKeyDescription = "The valid Esquio UI Api Key used for Esquio authentication.";

        public const string UriParameter = "--uri";
        public const string UriDescription = "The Esquio UI url base path.";

        public const string SpecifySubCommandErrorMessage = "You must specify at a subcommand";

        public const ConsoleColor ErrorColor = ConsoleColor.Red;
        public const ConsoleColor SuccessColor = ConsoleColor.Green;
    }
}
