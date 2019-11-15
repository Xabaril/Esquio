Esquio CLI
==========

**Esquio.CLI** is a simple crossplattform *dotnet tool* that allow you to interact with *Esquio UI* from the command line. This tool can be used from build or release pipelines on Azure DevOps, Github Actions etc.

To install **Esquio.CLI** open a console window and type the following command using the *.NET Core CLI*::

        dotnet tools install -g Esquio.Cli

::

Usage: dotnet-esquio [options] [command]

Options:
  -?|-h|--help  Show help information

Commands:
  **features**  
    *Manage Esquio features using Esquio UI HTTP API*

  **parameters**    
    *Manage Esquio parameters using Esquio UI HTTP API*

  **products**      
    *Manage Esquio products using Esquio UI HTTP API*

  **toggles**       
    *Manage Esquio toggles using Esquio UI HTTP API*

Run 'dotnet-esquio [command] --help' for more information about a command.