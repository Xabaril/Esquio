Introduction
============

A **Toggle** is code which defines when a feature is enabled or not. Each feature can use one or multiple toggles at the same time. In Esquio you have many different toggles out of the box, and of course you can write your custom toggles.

OnToogle
^^^^^^^^
One of the most straightforward toggle, which basically means that the feature should be enabled to everyone::

                {
                    "Name": "AnimationsMatch",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.Toggles.OnToggle"
                        }
                    ]
                }



OffToogle
^^^^^^^^^
One of the most straightforward toggle, which basically means that the feature should be disabled to everyone::

                {
                    "Name": "AnimationsMatch",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.Toggles.OffToggle"
                        }
                    ]
                }

EnvironmentToggle
^^^^^^^^^^^^^^^^^
This toggle allows you to enabled features dependending on the environment that an application is running in. This environment information is provided by `IEnvironmentNameProviderService <https://github.com/Xabaril/Esquio/blob/d666432f3f6fa1254dc852c7689485f1388b2da8/src/Esquio/Abstractions/Providers/IEnvironmentNameProviderService.cs#L9>`_. When you add Esquio to your application using ``AddEsquio()`` method, by default Esquio register a `NoEnvironmentNameProviderService <https://github.com/Xabaril/Esquio/blob/d666432f3f6fa1254dc852c7689485f1388b2da8/src/Esquio/Abstractions/Providers/IEnvironmentNameProviderService.cs#L18>`_. For ASP.NET Core projects, Esquio provides aa method called ``AddAspNetCoreDefaultServices`` that register by default an `AspNetEnvironmentNameProviderService <https://github.com/Xabaril/Esquio/blob/d666432f3f6fa1254dc852c7689485f1388b2da8/src/Esquio.AspNetCore/Providers/AspNetEnvironmentNameProviderService.cs#L8>`_ based on Microsoft.AspNetCore.Hosting.Abstractions.IWebHostEnvironment.

**Parameters**

    * Environments: *List of environments separated by semicolon you want the feature toggle to be enabled for*

::

                {
                    "Name": "MinutesRealTime",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.Toggles.EnvironmentToggle",
                            "Parameters": 
                            {
                                "Environments": "Staging;Production"
                            }
                        }
                    ]
                }
