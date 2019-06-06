Terminology
===========

The documentation and object model use a certain terminology that you should be aware of.

Product
^^^^^^^^
Allows you to manage multiple different software projects, for example, one solution can contains a web application and windows application that need the same set of features. Each product has its own unique set of features.

Feature
^^^^^^^
Features are characteristics of your product that describe its appearance, components, and capabilities. A feature is a slice of business functionality that has a corresponding benefit or set of benefits for that product's end user. Each feature has its own set of toggles.

Toggle
^^^^^^
Toggles allows you to control when a feature is enabled or not. Esquio provides many toggles out-of-the-box such us percentage rollouts, target specific users or environments, expiration dates or even hit the 'kill' switch for a feature programmatically.

Parameter
^^^^^^^^^
Parameters are variables that toggles need in their validation process.

Store
^^^^^
A mechanisim to allow you to store persistent the Esquio's object model such us products, features, toggles, parameters. Esquio provides out of the box two stores:

- ASP.NET Core JSON Configuration Provider.
- Entity Framework Core.