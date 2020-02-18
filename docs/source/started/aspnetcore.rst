Getting started with Esquio in ASP.NET Core
============================================

In this article, we are going to see how easy it is to use Esquio in your ASP.NET Core application using the NuGet packages provided by Xabaril.

> In `samples/WebApp <https://github.com/Xabaril/Esquio/tree/master/samples/WebApp>`_ you'll find a complete Esquio example in ASP.NET Core.

Setup
^^^^^

To install Esquio open a console window and type the following command using the .NET Core CLI::

        dotnet add package Esquio.Configuration.Store
        dotnet add package Esquio.AspNetCore


or using Powershell or Package Manager::

        Install-Package Esquio.Configuration.Store
        Install-Package Esquio.AspNetCore

or install via NuGet.

In the **ConfigureServices** method of Startup.cs, register the Esquio services::

        services
          .AddEsquio()
          .AddAspNetCoreDefaultServices()
          .AddConfigurationStore(Configuration, "Esquio");

``AddEsquio`` and ``AddAspNetCoreDefaultServices`` methods allows you to register the set of services that Esquio needs to works. The ``AddConfigurationStore`` method registers the configuration store to use, in this case, based on the default configuration system of `ASP.NET Core <https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.2>`_

Add the content below to your ``appsettings.json`` file::

        {
          "Esquio": {
            "Products": [
              {
                "Name": "default",
                "Features": [
                  {
                    "Name": "Colored",
                    "Enabled": true,
                    "Toggles": []
                  }
                ]
              }
            ]
          }
        }

ASP.NET Core Web Apps
^^^^^^^^^^^^^^^^^^^^^^^
When working with Esquio you can attach feature metadata to an endpoint. We do this using the route mappings configuration fluent API ``RequireFeature`` method::

        app.UseEndpoints(routes =>
        {      
            routes.MapControllerRoute(
              name: "default",
              pattern: "{controller=Match}/{action=Index}/{id?}").RequireFeature("HiddenGem");  
        });

You can specify many features separated by comma, so you can restrict access to the endpoints if a feature or a group features are enabled or not.

If you want more fine-grained control over your Controllers, Esquio provides a ``FeatureFilter`` attribute that forces you to supply a comma separated list of features names. You can specifies that access to a controller or action method is restricted to users if theses features are enabled or not::

        [FeatureFilter(Names = Flags.MinutesRealTime)]
        public IActionResult DetailLive()
        {
            return View();
        }

Also, you can use ``FeatureFilter`` to act as an Action constraint. You can create two Actions with the same ``ActionName`` and decorate one with ``FeatureFilter`` attribute to match the action only when the predefined feature name values are enabled or not.::

        [ActionName("Detail")]
        public IActionResult DetailWhenFlagsIsNotActive()
        {
            return View();
        }

        [FeatureFilter(Names = Flags.MinutesRealTime)]
        [ActionName("Detail")]
        public IActionResult DetailWhenFlagsIsActive()
        {
            return View();
        }

Sometimes you will need to configure a fallback action. Esquio provides an ``AddEndpointFallback`` method that accepts a ``RequestDelegate`` in order to configure your custom fallback::

        services
          .AddEsquio()
          .AddAspNetCoreDefaultServices()
          .AddConfigurationStore(Configuration, "Esquio")
          .AddEndpointFallback((context) => 
          {
              context.Response.StatusCode = StatusCodes.Status404NotFound;

              return Task.CompletedTask;
          })

Out-of-the-box Esquio provides ``EndpointFallbackAction`` class that defines common fallback actions to be used when no matching endpoints found:

    * Redirect result to MVC action::
        
        public static RequestDelegate RedirectToAction(string controllerName, string actionName)

    * Redirect result::
        
        public static RequestDelegate RedirectTo(string uri)

    * NotFound status response::
        
        public static RequestDelegate NotFound()

ASP.NET Core MVC
^^^^^^^^^^^^^^^^

With **ASP.NET MVC Core** we can use the ``FeatureTagHelper`` inside our Razor views to show or hide Razor fragments depending on feature is enabled or not.

.. code-block:: html

    <feature names="@Flags.MatchScore">
        <span class="badge badge-secondary badge-pill">@match.ScoreLocal - @match.ScoreVisitor</span>
    </feature>

In this example, if the feature **MatchScore** is enabled, you can show a new design of the match score. Names property is comma-separated list of feature names to be evaluated. If any feature is not active, the tag helper will suppress the content.

The ``FeatureTagHelper`` supports ``Include`` and ``Exclude`` attributes:

    * Include: *A comma-separated list of feature names to be evaluated. If any feature is not active, this tag helper suppresses  the content.*
    * Exclude: *A comma-separated list of feature names to be evaluated. If any feature is active, this tag helper suppresses the content.*

SPA and Native Apps
^^^^^^^^^^^^^^^^^^^^^^

Single-Page-Applications and native apps are becoming the new wave for modern applications. The challenge with feature flags in these kinds of applications is handling the state transformations. In case of SPAs the changes in a webpage's DOM and the platform specific controls in native apps.
We will need an endpoint to query if a feature or a set of features are enabled or not in order make real time personalization in the UX for example.

To enable this endpoint, in the ``Configure`` method, insert the middleware to expose the Esquio endpoint::

        app.UseEndpoints(routes =>
        {
            routes.MapEsquio(pattern: "esquio");
        });

Now you can start your application and check out your features at http(s)://server:port/esquio?featureName=Colored::

        [
          {
            "enabled": true,
            "name": "Colored"
          }
        ]

To disable the feature, change the ``appsettings.json``::

        "Enabled": false,

Test again the app::

        [
          {
            "enabled": false,
            "name": "Colored"
          }
        ]
