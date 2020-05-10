Configuration Store
===================

It is the simplest way to store your Esquio configuration. However, it is not recommended for production, only for small projects or for testing purposes.

Installation
^^^^^^^^^^^^

Install ``Esquio.AspNetCore`` package, typing the following command using the .NET Core CLI::

        dotnet add package Esquio.Configuration.Store

or using Powershell or Package Manager::

        Install-Package Esquio.Configuration.Store

or install via NuGet.

And register the specific service for this store::

    public class Startup
    {
        IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddEsquio()
                .AddConfigurationStore(_configuration);
        }

``AddConfigurationStore`` method registers the configuration store to use, in this case, based on the default configuration system of `ASP.NET Core <https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.2>`_

Setting your values
^^^^^^^^^^^^^^^^^^^

So, let's open our ``appsettings.json`` file. To help us in this task, we can use the Esquio schema, selecting it on the ``Schema`` options:

.. image:: ../images/esquioschema.png

Add the content below to your ``appsettings.json`` file::

        {
          "Esquio": {
            "Products": [
              {
                "Name": "default",
                "Features": [
                  {
                    "Name": "HiddenGem",
                    "Enabled": true,
                    "Toggles": []
                  }
                ]
              }
            ]
          }
        }

By default, ``Esquio`` will be the root element. However, you could change it on adding the configurationStore::

                .AddConfigurationStore(_configuration, key: "MyNewCustomRoot");
