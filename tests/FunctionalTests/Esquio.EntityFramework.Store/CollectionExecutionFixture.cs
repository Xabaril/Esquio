using Xunit;

namespace FunctionalTests.Esquio.EntityFramework.Store
{
    [CollectionDefinition(nameof(CollectionExecutionFixture))]
    public class CollectionExecutionFixture
        : ICollectionFixture<Fixture>
    {

    }
}
