# Esquio Extensions

This Azure DevOps Extension includes several Azure DevOps Pipelines tasks for Feature Toggles package [Esquio](https://esquio.readthedocs.io/en/latest/).

Please read [Esquio readthedocs](https://esquio.readthedocs.io/en/latest/) first to fully understand Esquio Feature Toggle package configuration and possibilities.

The tasks included in this version are:

- **Rollout feature:** With this task you can enable a feature in an Azure DevOps Pipeline.
- **Rollback feature:** This task is used to disable (rollback) a feature during an Azure DevOps Pipeline.
- **Set toggle parameter:** Some Esquio toggles accepts parameters, like for example a gradual rollout of a particular feature based in a percentage and a partition identifier. This task is used to change any parameter value of a particular toggle.

## Service connection

Before being able to use any of this tasks you need to create an Esquio [Service Connection](https://docs.microsoft.com/en-us/azure/devops/pipelines/library/service-endpoints?view=azure-devops&tabs=yaml) (enabled after installing the extension) and configure an API key in Esquio to use it with the service connection.

![Service connection](images/service-connection.png)

The parameters needed are:

- Name for the connection to be used in the Azure DevOps Pipelines
- Esquio API URL
- API Token, API key configured in Esquio.

Once you enter all the values you can click on *Verify connection* to check if the connection is well configured.

## Rollout feature with Esquio task

![Rollout feature with Esquio](images/rollout.png)

Parameters for this task are:

- **Esquio service endpoint:** This is a picklist to select the desired Esquio Service Connection.
- **Esquio Product:** Picklist to select any of the configured products in Esquio.
- **Esquio feature:** Picklist to select the Esquio feature desired to enable.

## Rollback feature with Esquio task

![Rollback feature with Esquio](images/rollback.png)

Parameters for this task are:

- **Esquio service endpoint:** This is a picklist to select the desired Esquio Service Connection.
- **Esquio Product:** Picklist to select any of the configured products in Esquio.
- **Esquio feature:** Picklist to select the Esquio feature desired to rollback.

## Set toggle parameter with Esquio

![Set toggle parameter with Esquio](images/set-parameter.png)

Parameters for this task are:

- **Esquio service endpoint:** This is a picklist to select the desired Esquio Service Connection.
- **Esquio Product:** Picklist to select any of the configured products in Esquio.
- **Esquio feature:** Picklist to select the Esquio feature desired to rollback.
- **Esquio toggle:** Picklist for Esquio toggle for the feature to set a particular parameter.
- **Esquio parameter:** Picklist for the Esquio parameter, associated with the toggle, to set the value.
- **Esquio parameter value:** New value for the Esquio Toggle parameter.

