![Esquio Build](https://github.com/xabaril/Esquio/workflows/Esquio%20Continous%20Integration/badge.svg?branch=master)

![Esquio Build](https://github.com/xabaril/Esquio/workflows/Esquio%20Nightly%20Build/badge.svg?branch=master)

[![Documentation Status](https://readthedocs.org/projects/esquio/badge/?version=latest)](https://esquio.readthedocs.io/en/latest/?badge=latest)

[![Blimp demo badge](https://blimpup.io/demo-badge.svg?repo=https://github.com/Xabaril/Esquio)](https://blimpup.io/preview-env/?repo=https://github.com/Xabaril/Esquio&port=app:80&port=ui:80&composeFiles=build/docker-compose-demo-with-ui-sqlserver-with-reference-images.yml)

## About [Esquio](https://esquio.readthedocs.io) 

Esquio is a [Feature Toggles (aka Feature Flags)](https://martinfowler.com/articles/feature-toggles.html) and A/B testing framework for .NET Core 3.0. Feature Toogle is a powerful technique that allows developers to deliver new functionality to users withouth changing code. Provides an alternative to to mantain multiples branches (aka feature branches), so any feature can be tested even before it is completed and ready for the release. We can release a version of our product with not production ready features. These non production ready features are hidden (toggled) for the broader set of users but can be enabled to any subset of testing or internal users we want them to try the features.We can use feature toogling to enable or disable features during run time.

Esquio is built with the possibility of use it not only in ASP.NET Core 3.0 in mind, but making it possible to use also in other .NET Core 3.0 projects like workers, webjobs, classlibraries, ... almost any kind of .NET Core 3.0 project. For the Esquio team, this is not only about using a library, but using a full Feature Toggles framework for all of our projects, and as a delivery mechanism.

We believe Feature Toggling is, somekind, a way of delivering software, making it a first class citizen in your DevOps processes, therefore we are working hard towards integrating it, via extension and pipelines tasks, with Azure DevOps, so you can use Esquio Toggles directly in your releases and delivery flows. Having a full toggle delivery experience.

Esquio Azure DevOps extensions are built in top of the Esquio API, in the case you need to integrate Esquio with any other tool, you can always use this API to handle the toggles.

Additionally, if you need it, Esquio has a full UI developed, so you can be able to handle all your Toggles in it, making it fairly simple to use and manage.

Maintained by [awesome community contributors](https://github.com/Xabaril/Esquio/graphs/contributors).


For project documentation, please visit [readthedocs](https://esquio.readthedocs.io).


## Temporary demo environment

If you want to play around with Esquio without running it locally, you can [boot a personal demo copy](https://blimpup.io/preview-env/?repo=https://github.com/Xabaril/Esquio&port=app:80&port=ui:80&composeFiles=build/docker-compose-demo-with-ui-sqlserver-with-reference-images.yml) from your browser without downloading or setting up anything.

Clicking the [link](https://blimpup.io/preview-env/?repo=https://github.com/Xabaril/Esquio&port=app:80&port=ui:80&composeFiles=build/docker-compose-demo-with-ui-sqlserver-with-reference-images.yml) boots this repo in the Blimp cloud, and creates a public URL for you to access it.

To use the demo, you can follow along with [this presentation at the ASP.NET Community Standup](https://www.youtube.com/watch?v=qotnVlgYd8c&t=1093). You can skip the docker-compose step and simply use the "Connect" buttons in the sandbox to access the demo app and the Esquio UI.

## How to build
Esquio is built against the latest NET Core 3.

* Run [install-sdk.ps1](https://github.com/Xabaril/Esquio/blob/master/install-sdk.ps1) or [install-sdk.sh](https://github.com/Xabaril/Esquio/blob/master/install-sdk.sh) to install required .NET Core SDK.
* Run [build.ps1](https://github.com/Xabaril/Esquio/blob/master/build.ps1) or [build.sh](https://github.com/Xabaril/Esquio/blob/master/build.sh) in the root of the repo to restore package, build solution and run tests.

## How to run migrations
### For SqlServer
<code>dotnet ef migrations add *MigrationName* --context StoreDbContext --project src/Esquio.UI.Api --output-dir Infrastructure/Data/Migrations/SqlServer</code>

### For Postgres
<code>dotnet ef migrations add *MigrationName* --context NpgSqlContext --project src/Esquio.UI.Api --output-dir Infrastructure/Data/Migrations/NpgSql</code>

### For MySql
<code>dotnet ef migrations add *MigrationName* --context MySqlContext --project src/Esquio.UI.Api --output-dir Infrastructure/Data/Migrations/MySql</code>

## Acknowledgements
Esquio is built using the following great open source projects and free services:

* [ASP.NET Core](https://github.com/aspnet)
* [XUnit](https://xunit.github.io/)
* [Fluent Assertions](http://www.fluentassertions.com/)
* [Fluent Validations](https://github.com/JeremySkinner/FluentValidation)
* [MediatR](https://github.com/jbogard/MediatR)
* [Problem Details](https://www.nuget.org/packages/Hellang.Middleware.ProblemDetails)
* [Serilog](https://github.com/serilog/serilog)
* [Swashbucke.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
* [Acheve](https://github.com/Xabaril/Acheve.TestHost)

..and last but not least a big thanks to all our [contributors](https://github.com/Xabaril/Esquio/graphs/contributors)!

## Code of conduct

This project has adopted the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/about/code-of-conduct/).

## Videos

ASP.NET Community Standup - May 12th 2020 - Esquio Feature Toggles

[![ASP.NET Community Standup - May 12th 2020 - Esquio Feature Toggles](https://img.youtube.com/vi/qotnVlgYd8c/0.jpg)](https://youtu.be/qotnVlgYd8c?list=PL1rZQsJPBU2St9-Mz1Kaa7rofciyrwWVx&t=225)

MeetUp | NetCore y Feature Flags(Spanish)

[![MeetUp | NetCore y Feature Flags(Spanish)](https://img.youtube.com/vi/VCGZZOFaPL0/0.jpg)](https://www.youtube.com/watch?v=VCGZZOFaPL0)
