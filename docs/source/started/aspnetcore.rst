Getting started with Esquio in ASP.NET Core
============================================

In this article, we are going to see how easy it is to use Esquio in your ASP.NET Core application using the NuGet packages provided by Xabaril.

> In `samples/WebApp <https://github.com/Xabaril/Esquio/tree/master/samples/WebApp>`_ you'll find a complete Esquio example in ASP.NET Core.

Installation
^^^^^^^^^^^^

To install Esquio open a console window and type the following command using the .NET Core CLI::

        dotnet package add Esquio.AspNetCore


or using Powershell or Package Manager::

        Install-Package Esquio.AspNetCore

or install via NuGet.

ASP.NET Core middleware
^^^^^^^^^^^^^^^^^^^^^^^

In the **ConfigureServices** method of Startup.cs, register the Esquio services::

        services
          .AddEsquio()
          .AddAspNetCoreDefaultServices()
          .AddConfigurationStore(Configuration, "Esquio");

``AddEsquio`` and ``AddAspNetCoreDefaultServices`` methods allows you to register the set of services that Esquio needs to works. The ``AddConfigurationStore`` method registers the configuration store to use, in this case, based on the default configuration system of [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.2)

In the ``Configure`` method, insert middleware to expose the Esquio JSON endpoint::

        app.UseEndpoints(routes =>
        {
            routes.MapEsquio(pattern: "esquio");
        });

Add the content below to your ``appsettings.json`` file::

        {
          "Esquio": {
            "Products": [
              {
                "Name": "Console",
                "Features": [
                  {
                    "Name": "Colored",
                    "Enabled": true,
                    "Toggles": [
                      {
                        "Type": "Esquio.Toggles.OnToggle"
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }

Now you can start your application and check out your features at http(s)://server:port/esquio?productName=Console&featureName=Colored::

        [
          {
            "Enabled": true,
            "Name": "Colored"
          }
        ]

To disable the feature, change the ``appsettings.json``::

        "Enabled": false,

Test again the app::

        [
          {
            "Enabled": false,
            "Name": "Colored"
          }
        ]

This approach should be usefull for JavaScript or Native clients.

ASP.NET MVC
^^^^^^^^^^^

With **ASP.NET MVC Core** we have different options to check if a feature is enabled. For example we can use the ``FeatureTagHelper`` inside our Razor views to show or hide Razor fragments depending on feature is enabled or not.

``` csharp
<feature names="@Flags.MatchScore">
    <span class="badge badge-secondary badge-pill">@match.ScoreLocal - @match.ScoreVisitor</span>
</feature>
```

In this example, if the feature **MatchScore** is enabled, you can show a nee design of the match score. Names property is coma separated list of features names to be evaluated if any feature is not active and then the tag helper will suppress the content.
