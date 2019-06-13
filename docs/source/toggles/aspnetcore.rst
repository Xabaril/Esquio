Esquio ASP.NET Core toggles
===========================

In addition to the toggles that Esquio provides out of the box, Esquio.AspNetCore provides more toggles to work with ASP.NET Core applications.

ClaimValueToggle
^^^^^^^^^^^^^^^^
This toggle allows you to enabled features depending on the current claims of logged in users.

**Parameters**

    * ClaimType: *The claim type used to check value.*
    * ClaimValues: *The claim value to check, multiple items separated by ';'.*

::

                {
                    "Name": "AnimationsMatch",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.Toggles.ClaimValueToggle",
                            "Parameters": 
                            {
                                "ClaimType": "Company",
                                "ClaimValues": "Contoso;ACME"
                            }
                        }
                    ]
                }

GradualRolloutClaimValueToggle
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
This toggle allows you gradually enabled features to a percentage of logged in users. Stickiness is based on the claim type value. Esquio uses `Jenkins hash function <https://en.wikipedia.org/wiki/Jenkins_hash_function>`_ that guarantees to the user get the same experience across many devices and also assures that a user which is among the first 30% will also be among the first 50% of the users. 

**Parameters**

    * Percentage: *The percentage (0-100) you want to enable the feature toggle for.*
    * ClaimType: *The claim type used to get value to rollout.*

::

                {
                    "Name": "DarkMode",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.Toggles.GradualRolloutClaimValueToggle",
                            "Parameters": 
                            {
                                "Percentage": 50,
                                "ClaimType": "role"
                            }
                        }
                    ]
                }

GradualRolloutHeaderValueToggle
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
This toggle allows you gradually enabled features to a percentage of logged in users. Stickiness is based on the HTTP header value. Esquio uses `Jenkins hash function <https://en.wikipedia.org/wiki/Jenkins_hash_function>`_ that guarantees to the user get the same experience across many devices and also assures that a user which is among the first 30% will also be among the first 50% of the users. 

**Parameters**

    * Percentage: *The percentage (0-100) you want to enable the feature toggle for.*
    * HeaderName: *he name of the header used to get the value to rollout.*

::

                {
                    "Name": "DarkMode",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.Toggles.GradualRolloutHeaderValueToggle",
                            "Parameters": 
                            {
                                "Percentage": 50,
                                "HeaderName": "X-Tenant"
                            }
                        }
                    ]
                }