Welcome to Esquio
=================

.. image:: images/xabaril.png
   :align: center

**Esquio** is a `Feature Toggles (aka Feature Flags) <https://martinfowler.com/articles/feature-toggles.html>`_ and A/B testing library for ASP.NET Core 3.0. Feature toggling is a powerful technique that allows developers to deliver new functionality to users without changing code. Feature toggles provide an alternative to mantaining multiple branches (aka feature branches), so any feature can be tested even before it is completed and ready for the release. We can release a version of our product without production-ready features. These non production-ready features are hidden (toggled) for the broader set of users but can be enabled to any subset of testing or internal users we want to try out the features. We can even use feature toggling to enable or disable features during runtime.

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
   started/azuredevopstasks

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Feature documentation

   feature/behaviors

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Toggles

   toggles/esquio
   toggles/aspnetcore
   toggles/extensibility
