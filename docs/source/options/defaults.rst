Defaults
=========

Esquio allows to configure the Product and Deployment using specifed methods on EsquioOptions.

ConfigureDefaultProductName
^^^^^^^^^^^^^^^

Allow to configure default Product name to use, the default value is 'default'::

          services.AddEsquio(setup=>
            {
                setup.ConfigureDefaultProductName("default");
            });
     

ConfigureDefaultDeploymentName
^^^^^^^^^^^^^^^^

Allow to configure default Deployment name to use, the default value is 'Tests'::

          services.AddEsquio(setup=>
            {
                setup.ConfigureDefaultDeploymentName("Tests");
            });
     