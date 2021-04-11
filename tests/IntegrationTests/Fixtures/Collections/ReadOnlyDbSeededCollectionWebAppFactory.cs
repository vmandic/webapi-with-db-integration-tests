using Xunit;
using IntegrationTests.Fixtures;

namespace IntegrationTests.Fixtures.Collections
{
    [CollectionDefinition(nameof(ReadOnlyDbSeededCollectionWebAppFactory))]
    public class ReadOnlyDbSeededCollectionWebAppFactory : ICollectionFixture<ReadOnlyDbSeededWebAppFactory> { }
}