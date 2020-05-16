Esquio toggles
==============

A **Toggle** is piece of code that defines when a feature is enabled or not. Each feature can use one or more toggles at the same time, but never more than one toggle of the same type. In Esquio you have many different toggles out of the box, and of course you can write your custom toggles.


Environment Variable
^^^^^^^^^^^^^^^^^^^^
This toggle enables the feature if the value of the configured environment variable is in the list. 
This environment information is provided by `IEnvironmentNameProviderService <https://github.com/Xabaril/Esquio/blob/d666432f3f6fa1254dc852c7689485f1388b2da8/src/Esquio/Abstractions/Providers/IEnvironmentNameProviderService.cs#L9>`_. When you add Esquio to your application using ``AddEsquio()`` method, by default Esquio registers a `NoEnvironmentNameProviderService <https://github.com/Xabaril/Esquio/blob/d666432f3f6fa1254dc852c7689485f1388b2da8/src/Esquio/Abstractions/Providers/IEnvironmentNameProviderService.cs#L18>`_. For ASP.NET Core projects, Esquio provides a method called ``AddAspNetCoreDefaultServices`` that registers by default an `AspNetEnvironmentNameProviderService <https://github.com/Xabaril/Esquio/blob/d666432f3f6fa1254dc852c7689485f1388b2da8/src/Esquio.AspNetCore/Providers/AspNetEnvironmentNameProviderService.cs#L8>`_ based on Microsoft.AspNetCore.Hosting.Abstractions.IWebHostEnvironment.

**Type** 

    * Esquio.Toggles.EnvironmentToggle

**Parameters**

    * EnvironmentVariable: *The environment variable name.*
    * Values: *The values to activate this toggle separated by ';' character.*

::

                {
                    "Name": "MinutesRealTime",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.Toggles.EnvironmentToggle",
                            "Parameters": 
                            {
                                "EnvironmentVariable": "ASPNETCORE_ENVIRONMENT",
                                "Values": "Staging;Production"
                            }
                        }
                    ]
                }

Between dates
^^^^^^^^^^^^^
This toggle enables the feature when current UTC date falls within the interval.

**Type** 

    * Esquio.Toggles.FromToToggle

**Parameters**

    * From: *The interval start (yyyy-MM-dd HH:mm:ss) when this toggle is activated.*
    * To: *The interval end (yyyy-MM-dd HH:mm:ss) when this toggle is activated.*

::

                {
                    "Name": "DarkMode",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.Toggles.FromToToggle",
                            "Parameters": 
                            {
                                "From": "2019-06-12 00:00:00",
                                "To": "2019-06-14 23:59:59"
                            }
                        }
                    ]
                }

