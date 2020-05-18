Behaviors
=========

Esquio allows you to configure their behaviors when a feature does not exists or fails during evaluation. You can configure these behaviors by modifying the Esquio setup.

OnErrorBehavior
^^^^^^^^^^^^^^^

There are three options to configure when a feature fails during evaluation:

    * **OnErrorBehavior.Throw**: Re-throw the exception.
    
    * **OnErrorBehavior.SetDisabled**: Returns disabled as a result of the evaluation.

    * **OnErrorBehavior.SetEnabled**: Returns enabled as a result of the evaluation.

The ``AddEsquio`` method provides you a way to configure the behavior when a feature fails during evalution::

        services
          .AddEsquio(setup => setup.ConfigureOnErrorBehavior(OnErrorBehavior.Throw))
          .AddAspNetCoreDefaultServices()
          .AddConfigurationStore(Configuration, "Esquio");

In the above example, the exception will be thrown if some fail happens during the evalution process.

NotFoundBehavior
^^^^^^^^^^^^^^^^

There are three options to configure when a feature does not exists in the store:
    
    * **NotFoundBehavior.SetDisabled**: Returns disabled as a result of the evaluation.

    * **NotFoundBehavior.SetEnabled**: Returns enabled as a result of the evaluation.

The ``AddEsquio`` method provides you a way to configure the behavior when a feature does not exists in the store::

        services
          .AddEsquio(setup => setup.ConfigureNotFoundBehavior(NotFoundBehavior.SetDisabled))
          .AddAspNetCoreDefaultServices()
          .AddConfigurationStore(Configuration, "Esquio");

In the above example, if the feature does not exists in the store, Esquio will returns disabled as a result of the feature evaluation process.




