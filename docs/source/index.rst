Welcome to Esquio
=================

.. image:: images/xabaril.png
   :align: center

**Esquio** is a `Feature Toggles (aka Feature Flags) <https://martinfowler.com/articles/feature-toggles.html>`_ and A/B testing library for ASP.NET Core 3.0. Feature toggling is a powerful technique that allows developers to deliver new functionality to users without changing code. Feature toggles provide an alternative to mantaining multiple branches (aka feature branches), so any feature can be tested even before it is completed and ready for the release. We can release a version of our product without production-ready features. These non production-ready features are hidden (toggled) for the broader set of users but can be enabled to any subset of testing or internal users we want to try out the features. We can even use feature toggling to enable or disable features during runtime.

Esquio is built with the possibility of use it not only in ASP.NET Core 3.0 in mind, but making it possible to use also in other .NET Core 3.0 projects like workers, webjobs, classlibraries, ... almost any kind of .NET Core 3.0 project. For the Esquio team, this is not only about using a library, but using a full Feature Toggles framework for all of our projects, and as a delivery mechanism.

We believe Feature Toggling is, somekind, a way of delivering software, making it a first class citizen in your DevOps processes, therefore we are working hard towards integrating it, via extension and pipelines tasks, with Azure DevOps, so you can use Esquio Toggles directly in your releases and delivery flows. Having a full toggle delivery experience.

Esquio Azure DevOps extensions are built in top of the Esquio API, in the case you need to integrate Esquio with any other tool, you can always use this API to handle the toggles.

Additionally, if you need it, Esquio has a full UI developed, so you can be able to handle all your Toggles in it, making it fairly simple to use and manage.

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: About Esquio

   intro/terminology
   intro/contributing

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Getting Started Guide

   started/netcore
   started/aspnetcore
   started/worker
   started/azuredevopstasks
   started/diagnostics

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Esquio Options

   options/behaviors
   options/defaults
   options/scopedevaluation

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Toggles

   toggles/esquio
   toggles/aspnetcore
   toggles/extensibility

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Stores

   stores/configuration

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Tools

   tools/esquiocli
   tools/miniprofiler

