using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FunctionalTests.Esquio.EntityFramework.Store
{
    public class Fixture : IDisposable
    {
        public bool IsAppVeyorExecution { get; private set; }
        public List<DbContextOptions<StoreDbContext>> Options { get; set; }

        public Fixture()
        {
            IsAppVeyorExecution = Environment.GetEnvironmentVariable("Appveyor")?.ToUpperInvariant() == "TRUE";
        }

        public void Dispose()
        {
            foreach (var option in Options)
            {
                using (var context = new StoreDbContext(option, new StoreOptions()))
                {
                    context.Database.EnsureDeleted();
                }
            }
        }
    }
}
