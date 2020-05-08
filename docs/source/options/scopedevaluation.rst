Scoped Evaluation
=========

Esquio allows to cache, at scoped level ( the request on ASP.NET Core), the evalution result of any feature. This reduce the number of evaluation, better performance, and improve the consistency of evaluation results on the same scope.

UseScopedEvaluation
^^^^^^^^^^^^^^^

Allow to set the use of scoped evaluation results, on ASP.NET Core the scope is the request and the results are stored on the HTTP Context Items property::

          services.AddEsquio(setup=>
            {
                setup.UseScopedEvaluation(useScopedEvaluation: true);
            });
     
Customize Scoped Evaluation Holder
^^^^^^^^^^^^^^^

You can override the *IScopedEvaluationHolder* and create your own evaluation result holder implementing the interface ::

          public interface IScopedEvaluationHolder
          {
               Task<bool> TryGetAsync(string featureName, out bool enabled);
               Task SetAsync(string featureName, bool enabled);
          }