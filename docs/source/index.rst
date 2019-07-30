Welcome to Esquio
=================

.. image:: images/xabaril.png
   :align: center

**Esquio** is a `Feature Toggles (aka Feature Flags) <https://martinfowler.com/articles/feature-toggles.html>`_ and A/B testing library for ASP.NET Core 3.0. Feature Toogle is a powerful technique that allows developers to deliver new functionality to users withouth changing code. Provides an alternative to to mantain multiples branches (aka feature branches), so any feature can be tested even before it is completed and ready for the release. We can release a version of our product with not production ready features. These non production ready features are hidden (toggled) for the broader set of users but can be enabled to any subset of testing or internal users we want them to try the features.We can use feature toogling to enable or disable features during run time.

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
   :caption: Toggles

   toggles/esquio
   toggles/aspnetcore
   toggles/extensibility