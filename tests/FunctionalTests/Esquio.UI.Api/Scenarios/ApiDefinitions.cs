using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionalTests.Esquio.UI.Api.Scenarios
{
    public static class ApiDefinitions
    {
        public static class V1
        {
            public static class Flags
            {
                public static string Rollout(int productId, int featureId)
                {
                    return $"api/v1/product/{productId}/flags/{featureId}/rollout";
                }
                public static string Delete(int productId, int featureId)
                {
                    return $"api/v1/product/{productId}/flags/{featureId}";
                }
            }
        }
    }
}
