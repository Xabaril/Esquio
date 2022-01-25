using Respawn;
using Respawn.Graph;
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
                checkpoint = new Checkpoint()
                {
                    DbAdapter = checkpoint.DbAdapter,
                    TablesToIgnore = new Table[] { "__EFMigrationsHistory" },
                    WithReseed = false,
                    SchemasToInclude = new string[] { "public" }
                };
            }
            else if (checkpoint.DbAdapter == DbAdapter.MySql)
            {
                checkpoint = new Checkpoint()
                {
                    DbAdapter = checkpoint.DbAdapter,
                    TablesToIgnore = new Table[] { "__EFMigrationsHistory" },
                    WithReseed = false,
                };
            }
            else
            {
                //default is sql server configuration
                checkpoint = new Checkpoint()
                {
                    DbAdapter = checkpoint.DbAdapter,
                    TablesToIgnore = new Table[] { "__EFMigrationsHistory" },
                    WithReseed = true,
                };
            }
        }
    }
}
