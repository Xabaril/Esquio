using Respawn;

namespace FunctionalTests.Esquio.UI.Api.Seedwork
{
    static class DbAdapterFactory
    {
        public static IDbAdapter CreateFromProvider(string providerName)
        {
            return providerName switch
            {
                "SqlServer" => DbAdapter.SqlServer,
                "Npgsql" => DbAdapter.Postgres,
                _ => DbAdapter.SqlServer
            };
        }
    }
}
