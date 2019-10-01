Esquio toggles
==============

A **Toggle** is code which defines when a feature is enabled or not. Each feature can use one or multiple toggles at the same time, but never more than one toggle of the same type. In Esquio you have many different toggles out of the box, and of course you can write your custom toggles.

UserNameToggle
^^^^^^^^^^^^^^
This toggle allows you to enabled features to a specific set of logged in users.

**Parameters**

    * Users: *The collection of user(s) to activate it separated by ';' character.*

::

                {
                    "Name": "MinutesProgressBar",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.Toggles.UserNameToggle",
                            "Parameters": 
                            {
                                "Users": "betauser;beta"
                            }
                        }
                    ]
                }

RoleNameToggle
^^^^^^^^^^^^^^
This toggle allows you to enabled features to a specific set of logged in users that belong to a specific role.

**Parameters**

    * Users: *The collection of role(s) to activate this toggle separated by ';' character.*

::

                {
                    "Name": "MinutesProgressBar",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.Toggles.RoleNameToggle",
                            "Parameters": 
                            {
                                "Users": "betauser;beta"
                            }
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

FromToToggle
^^^^^^^^^^^^
This toggles allows you to enabled features dependending on current UTC time.

**Parameters**

    * From: *The from date (yyyy-MM-dd HH:mm:ss) interval when this toggle is activated.*
    * To: *The to date (yyyy-MM-dd HH:mm:ss) interval when this toggle is activated.*

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

GradualRolloutUserNameToggle
^^^^^^^^^^^^^^^^^^^^^^^^^^^^
This toggle allows you to gradually enable features to a percentage of logged in users. Stickiness is based on the user name. Esquio uses `Jenkins hash function <https://en.wikipedia.org/wiki/Jenkins_hash_function>`_ which guarantees the user gets the same experience across many devices and also ensures that a user who is among the first 30% will also be among the first 50% of users. 

**Parameters**

    * Percentage: *The percentage (0-100) you want to enable the feature toggle for.*

::

                {
                    "Name": "DarkMode",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.Toggles.GradualRolloutUserNameToggle",
                            "Parameters": 
                            {
                                "Percentage": 50
                            }
                        }
                    ]
                }

