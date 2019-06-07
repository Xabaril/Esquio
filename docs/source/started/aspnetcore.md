# Getting started with Esquio in ASP.NET Core

In this article, we are going to see how easy it is to use Esquio in your ASP.NET Core application using the NuGet packages provided by Xabaril.

> In [samples/WebApp](https://github.com/Xabaril/Esquio/tree/master/samples/WebApp) you'll find a complete Esquio example in ASP.NET Core.

## Installation

To install Esquio open a console window and type the following command using the .NET Core CLI:

```
dotnet package add Esquio.AspNetCore
```

or using Powershell or Package Manager:

```
Install-Package Esquio.AspNetCore
```

or install via NuGet.

## Setup

In the ConfigureServices method of Startup.cs, register the Esquio services:

```csharp
sevices
    .AddEsquio()
    .AddAspNetCoreDefaultServices()
    .AddConfigurationStore(Configuration, "Esquio");
```

In the Configure method, insert middleware to expose the Esquio JSON endpoint:

```csharp
app.UseEndpoints(routes =>
{
    routes.MapEsquio(pattern: "esquio");
});
```

Now you can start your application and check out your features at /esquio?productName=test&featureName=Menu

```json
[
  {
    "Enabled": false,
    "Name": "Menu"
  }
]
```
We will see how to configure Esquio sooner.