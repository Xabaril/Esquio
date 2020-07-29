Http Store
==========

Esquio Http store connects against a Http Api that has been previously deployed on a Esquio UI.

> In `samples/GettingStarted.AspNetCore.Mvc.HttpStore <https://github.com/Xabaril/Esquio/tree/master/samples/GettingStarted.AspNetCore.Mvc.HttpStore>`_ you'll find a full example of this store.


Installation
-------------

Install ``Esquio.Http.Store`` package, typing the following command using the .NET Core CLI::

        dotnet add package Esquio.Http.Store

or using Powershell or Package Manager::

        Install-Package Esquio.Http.Store

or install via NuGet.


In the ``ConfigureServices`` method of the ``Startup`` class, register the specific service for this store::

                AddEsquio()
                .AddHttpStore(options =>
                {
                    options
                        .UseBaseAddress("http://localhost:1368/") //this is Esquio UI base address
                        .UseApiKey("b6+KYpSY8VPMBmHLNJ00z80aPOe+Li4EGe4idoKKI1A="); // this is a Api Key on Esquio UI (only Reader permission is Required)
                });

``AddHttpStore`` method registers the http store to use. in this case, based on the default configuration system of `ASP.NET Core <https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.2>`_

And that's all. Log in Esquio UI and configure your toggles as you need.

Optionally, you can enable *cache* on http store configuration, this reduce the number of request to *Esquio UI* and improve the client performance. By default *cache* is disabled but this can be enabled and configured with UseCache extension method::

                AddEsquio()
                .AddHttpStore(options =>
                {
                    options
                        .UseBaseAddress("http://localhost:1368/") //this is Esquio UI base address
                        .UseApiKey("b6+KYpSY8VPMBmHLNJ00z80aPOe+Li4EGe4idoKKI1A=") // this is a Api Key on Esquio UI (only Reader permission is Required)
                        .UseCache(true, slidingExpiration: TimeSpan.FromSeconds(10));
                });


