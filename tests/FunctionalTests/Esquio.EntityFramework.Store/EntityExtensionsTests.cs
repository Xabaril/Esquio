using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FunctionalTests.Esquio.EntityFramework.Store
{
    [Collection(nameof(CollectionExecutionFixture))]
    class entityextensions_should
    {
        private readonly Fixture _fixture;

        public entityextensions_should(Fixture fixture)
        {
            _fixture = fixture;
        }
        //private readonly Fixture _fixture;

        //public entityextensions_should(Fixture fixture)
        //{
        //    _fixture = fixture;
        //    _fixture.Options = DbContextOptions<StoreDbContext>)o)).ToList();

        //    foreach (var options in _fixture.Options)
        //    {
        //        using (var context = new StoreDbContext(options, new StoreOptions()))
        //        {
        //            context.Database.EnsureCreated();

        //            AddData(context);
        //        }
        //    }
        //}

        public EntityFrameworkCoreFeaturesStore Build()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<EntityFrameworkCoreFeaturesStore>();

            var connectionString = _fixture.IsAppVeyorExecution ?
               $@"Server=(local)\SQL2016;Database=Test.Esquio.EntityFramework-3.0.0.Extensions;User ID=sa;Password=Password12!" :
               $@"Server=tcp:localhost,1833;Database=Test.Esquio.EntityFramework-3.0.0.Extensions;User ID=sa;Password=Password12!";

            var builder = new DbContextOptionsBuilder<StoreDbContext>();
            builder.UseSqlServer(connectionString);

            return new EntityFrameworkCoreFeaturesStore(new StoreDbContext(builder.Options, new StoreOptions()), logger);
        }
    }
}
