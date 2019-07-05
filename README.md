## About Esquio (UNDER DEVELOPMENT but almost finished!)

Esquio is a [Feature Toggles (aka Feature Flags)](https://martinfowler.com/articles/feature-toggles.html>) and A/B testing framework for ASP.NET Core 3.0. Feature Toogle is a powerful technique that allows developers to deliver new functionality to users withouth changing code. Provides an alternative to to mantain multiples branches (aka feature branches), so any feature can be tested even before it is completed and ready for the release. We can release a version of our product with not production ready features. These non production ready features are hidden (toggled) for the broader set of users but can be enabled to any subset of testing or internal users we want them to try the features.We can use feature toogling to enable or disable features during run time.

Founded and maintained by [Unai Zorrila Castro](https://twitter.com/_unaizc_), [Luis Ruiz Pavon](https://twitter.com/luisruizpavon), [Quique Fdez Guerra](https://twitter.com/CKGrafico) and [Luis Fraile](https://twitter.com/lfraile).

For project documentation, please visit [readthedocs](https://esquio.readthedocs.io).

## How to build
Esquio is built against the latest NET Core 3.

* [Install](https://www.microsoft.com/net/download/core#/current) the [required](https://github.com/Xabaril/Esquio/blob/master/global.json) .NET Core SDK
* Run [build.ps1](https://github.com/Xabaril/Esquio/blob/master/build.ps1) or [build.sh](https://github.com/Xabaril/Esquio/blob/master/build.sh) in the root of the repo.

## Acknowledgements
Esquio is built using the following great open source projects and free services:

* [ASP.NET Core](https://github.com/aspnet)
* [XUnit](https://xunit.github.io/)
* [Fluent Assertions](http://www.fluentassertions.com/)

..and last but not least a big thanks to all our [contributors](https://github.com/Xabaril/Esquio/graphs/contributors)!
