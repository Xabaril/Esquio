using Respawn;

namespace FunctionalTests.Esquio.UI.Api.Seedwork
{
    static class DbAdapterFactory
    {
        public static IDbAdapter CreateFromProvider(string providerName)
        {
            return providerName.ToLowerInvariant() switch
            {
                "sqlserver" => DbAdapter.SqlServer,
                "npgsql" => DbAdapter.Postgres,
                _ => DbAdapter.SqlServer
            };
        }
    }
}
