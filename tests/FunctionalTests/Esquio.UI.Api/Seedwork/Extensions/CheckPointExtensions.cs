using Respawn;
using System;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Extensions
{
    static class CheckPointExtensions
    {
        public static void ConfigureForCurrentAdapter(this Checkpoint checkpoint)
        {
            _ = checkpoint ?? throw new ArgumentNullException(nameof(checkpoint));

            if ( checkpoint.DbAdapter == DbAdapter.Postgres)
            {
                checkpoint.TablesToIgnore = new string[] { "__EFMigrationsHistory" };
                checkpoint.WithReseed = false;
                checkpoint.SchemasToInclude = new string[] { "public" };
               
            }
            else if (checkpoint.DbAdapter == DbAdapter.MySql)
            {
                checkpoint.WithReseed = false;
                checkpoint.TablesToIgnore = new string[] { "__EFMigrationsHistory" };
                //checkpoint.TablesToInclude = new string[] { "ApiKeys" };

            }
            else
            {
                //default is sql server configuration
                checkpoint.TablesToIgnore = new string[] { "__EFMigrationsHistory" };
                checkpoint.WithReseed = true;
            }
        }
    }
}
