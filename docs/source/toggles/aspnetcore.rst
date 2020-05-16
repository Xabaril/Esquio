Esquio ASP.NET Core toggles
===========================

In addition to the toggles that Esquio provides out of the box, Esquio.AspNetCore provides more toggles to work with ASP.NET Core applications.

Identity Claim Value
^^^^^^^^^^^^^^^^^^^^
This toggle enables its feature if the identity claim of the current user exists and its value is in the list.

**Type**

    * Esquio.Toggles.ClaimValueToggle

**Parameters**

    * ClaimType: *The claim type name.*
    * ClaimValues: *The claim values to activate this toggle separated by ';' character.*

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


Client IP Address
^^^^^^^^^^^^^^^^^
This toggle enables its feature if the client IP address is in the list.

**Type** 

    * Esquio.Toggles.ClientIpAddressToggle

**Parameters**

    * IpAddresses: *The IP addresses to activate this toggle separated by ';' character.*

::

                {
                    "Name": "SecretZoneMatch",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.Toggles.ClientIpAddressToggle",
                            "Parameters": 
                            {
                                "ClaimType": "IpAddresses",
                                "ClaimValues": "11.22.44.88;11.22.33.44"
                            }
                        }
                    ]
                }

Partial rollout by Identity Claim value
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
This toggle enables its feature The claim exists and its value falls within the percentage created by Esquio Partitioner. 
Stickiness is based on the claim type value. Esquio uses `Jenkins hash function <https://en.wikipedia.org/wiki/Jenkins_hash_function>`_ that guarantees to the user get the same experience across many devices and also assures that a user which is among the first 30% will also be among the first 50% of the users. 

**Type** 

    * Esquio.AspNetCore.Toggles.GradualRolloutClaimValueToggle  

**Parameters**

    * Percentage: *The percentage of users that activates this toggle. Percentage from 0 to 100.*
    * ClaimType: *The identity claim type used whom value is used by Esquio Partitioner.*

::

                {
                    "Name": "DarkMode",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.AspNetCore.Toggles.GradualRolloutClaimValueToggle",
                            "Parameters": 
                            {
                                "Percentage": 50,
                                "ClaimType": "role"
                            }
                        }
                    ]
                }

Partial rollout by Http Header value
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
This toggle enables its feature when the request header exists and its value falls within percentage created by Esquio Partitioner. 
Stickiness is based on the HTTP header value. Esquio uses `Jenkins hash function <https://en.wikipedia.org/wiki/Jenkins_hash_function>`_ that guarantees to the user get the same experience across many devices and also assures that a user which is among the first 30% will also be among the first 50% of the users. 


**Type** 

    * Esquio.AspNetCore.Toggles.GradualRolloutHeaderValueToggle

**Parameters**

    * Percentage: *The percentage of users that activates this toggle. Percentage from 0 to 100.*
    * HeaderName: *The header name used whom value is used by Esquio Partitioner.*

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

Partial rollout by Http Session Id
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
This toggle enables its feature if the session identifier falls within percentage created by Esquio Partitioner.
Stickiness is based on the ASP.NET Core SessionId value. 

**Type** 

    * Esquio.AspNetCore.Toggles.GradualRolloutSessionToggle

**Parameters**

    * Percentage: *The percentage of sessions that activates this toggle. Percentage from 0 to 100.*

::

                {
                    "Name": "DarkMode",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.AspNetCore.Toggles.GradualRolloutSessionToggle",
                            "Parameters": 
                            {
                                "Percentage": 50
                            }
                        }
                    ]
                }

Partial rollout by UserName
^^^^^^^^^^^^^^^^^^^^^^^^^^^
This toggle allows the current user name falls within percentage created by Esquio Partitioner.
Stickiness is based on the user name. Esquio uses `Jenkins hash function <https://en.wikipedia.org/wiki/Jenkins_hash_function>`_ which guarantees the user gets the same experience across many devices and also ensures that a user who is among the first 30% will also be among the first 50% of users. 

**Type** 

    * Esquio.AspNetCore.Toggles.GradualRolloutUserNameToggle

**Parameters**

    * Percentage: *The percentage of users that activates this toggle. Percentage from 0 to 100.*

::

                {
                    "Name": "DarkMode",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.AspNetCore.Toggles.GradualRolloutUserNameToggle",
                            "Parameters": 
                            {
                                "Percentage": 50
                            }
                        }
                    ]
                }

Http Header value
^^^^^^^^^^^^^^^^^
This toggle enables its feature if the request header exists and its value its in the list.

**Type** 

    * Esquio.AspNetCore.Toggles.HeaderValueToggle

**Parameters**

    * HeaderName: *The header name.*
    * HeaderValues: *The header values to activate this toggle separated by ';' character.*

::

                {
                    "Name": "MinutesProgressBar",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.AspNetCore.Toggles.HeaderValueToggle",
                            "Parameters": 
                            {
                                "HeaderName": "Accept-Language",
                                "HeaderValues": "en-US;es-ES"
                            }
                        }
                    ]
                }

Environment
^^^^^^^^^^^
This toggle enables its feature if the host execution environment and its value is in the list.

**Type** 

    * Esquio.AspNetCore.Toggles.HostEnvironmentToggle

**Parameters**

    * Environments: *The ASP.NET Core host environments to activate this toggle separated by ';' character.*

::

                {
                    "Name": "MinutesProgressBar",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.AspNetCore.Toggles.HostEnvironmentToggle",
                            "Parameters": 
                            {
                                "Environments": "Staging;Production"
                            }
                        }
                    ]
                }

Host name
^^^^^^^^^
This toggle enables its feature if the hostname of the client instance is in the list.

**Type** 

    * Esquio.AspNetCore.Toggles.HostNameToggle

**Parameters**

    * HostNames: *The request connection hostnames values to activate this toggle separated by ';' character.*

::

                {
                    "Name": "MinutesProgressBar",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.AspNetCore.Toggles.HostNameToggle",
                            "Parameters": 
                            {
                                "Environments": "mycompany.org;en.domain.com"
                            }
                        }
                    ]
                }


Country
^^^^^^^
This toggle enables its feature if the request country is in the list (Ip geolocation through https://ip2c.org service).

**Type** 

    * Esquio.AspNetCore.Toggles.Ip2CountryToggle

**Parameters**

    * Countries: *The request country values (two letters, ISO 3166) to activate this toggle separated by ';' character.*

::

                {
                    "Name": "MinutesProgressBar",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.AspNetCore.Toggles.Ip2CountryToggle",
                            "Parameters": 
                            {
                                "Environments": "ES;IT"
                            }
                        }
                    ]
                }

Identity Role
^^^^^^^^^^^^^
This toggle enables its feature if the identity role is in the list.

**Type** 

    * Esquio.AspNetCore.Toggles.RoleNameToggle

**Parameters**

    * Roles: *The identity role values to activate this toggle separated by ';' character.*

::

                {
                    "Name": "MinutesProgressBar",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.AspNetCore.Toggles.RoleNameToggle",
                            "Parameters": 
                            {
                                "Users": "betauser;beta"
                            }
                        }
                    ]
                }

Server IP
^^^^^^^^^
This toggle enables its feature if the host IP address is in the list.

**Type** 

    * Esquio.AspNetCore.Toggles.ServerIpAddressToggle

**Parameters**

    * IpAddresses: *The host IP adddresses to activate this toggle separated by ';' character.*

::

                {
                    "Name": "MinutesProgressBar",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.AspNetCore.Toggles.ServerIpAddressToggle",
                            "Parameters": 
                            {
                                "Users": "11.22.44.88;11.22.33.44"
                            }
                        }
                    ]
                }

User Agent
^^^^^^^^^^
This toggle enables its feature if the request user agent browser is in the list.

**Type** 

    * Esquio.AspNetCore.Toggles.UserAgentToggle

**Parameters**

    * Browsers: *The user agents to activate this toggle separated by ';' character.*

::

                {
                    "Name": "MinutesProgressBar",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.AspNetCore.Toggles.UserAgentToggle",
                            "Parameters": 
                            {
                                "Users": "Mozilla/5.0;Chrome/81.0.4"
                            }
                        }
                    ]
                }


Identity name
^^^^^^^^^^^^^
This toggle enables its feature if the identity name is in the list.

**Type** 

    * Esquio.AspNetCore.Toggles.UserNameToggle

**Parameters**

    * Users: *The identity names to activate this toggle separated by ';' character.*

::

                {
                    "Name": "MinutesProgressBar",
                    "Enabled": true,
                    "Toggles": [
                        {
                            "Type": "Esquio.AspNetCore.Toggles.UserNameToggle",
                            "Parameters": 
                            {
                                "Users": "betauser;beta"
                            }
                        }
                    ]
                }





