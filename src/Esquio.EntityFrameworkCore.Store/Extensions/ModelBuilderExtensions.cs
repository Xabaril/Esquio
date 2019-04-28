using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    internal static class ModelBuilderExtensions
    {
        internal static EntityTypeBuilder<TEntity> ToTable<TEntity>(
            this EntityTypeBuilder<TEntity> entityTypeBuilder,
            TableConfiguration configuration)
            where TEntity : class
        {
            return string.IsNullOrWhiteSpace(configuration.Schema) ? entityTypeBuilder.ToTable(configuration.Name) : entityTypeBuilder.ToTable(configuration.Name, configuration.Schema);
        }

        internal static IEnumerable<Type> GetMappingTypes(this Assembly assembly, Type mappingInterface)
        {
            return assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.GetInterfaces()
                .Any(y => y.GetTypeInfo().IsGenericType && y.GetGenericTypeDefinition() == mappingInterface));
        }

        public static ModelBuilder ApplyConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly, StoreOptions storeOptions)
        {
            var mappingTypes = assembly.GetMappingTypes(typeof(IEntityTypeConfiguration<>));

            var entityMethod = typeof(ModelBuilder)
                .GetMethods()
                .Single(x => x.Name == "Entity" && x.IsGenericMethod && x.ReturnType.Name == "EntityTypeBuilder`1");

            foreach (var mappingType in mappingTypes)
            {
                var genericTypeArg = mappingType.GetInterfaces().Single().GenericTypeArguments.Single();
                var genericEntityMethod = entityMethod.MakeGenericMethod(genericTypeArg);
                var entityBuilder = genericEntityMethod.Invoke(modelBuilder, null);
                var mapper = Activator.CreateInstance(mappingType, storeOptions);
                mapper.GetType().GetMethod("Configure").Invoke(mapper, new[] { entityBuilder });
            }

            return modelBuilder;
        }
    }
}