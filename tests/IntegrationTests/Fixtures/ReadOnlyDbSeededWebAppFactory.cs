using WebApi;

namespace IntegrationTests.Fixtures
{
    /// <summary>
    /// A database fixture which appends "-ReadOnly" to the active connection string to create and use a "read-only" operations database suited for testing purposes.
    /// </summary>
    public class ReadOnlyDbSeededWebAppFactory : CustomWebAppFactory<Startup>
    {
        public ReadOnlyDbSeededWebAppFactory()
        {
            AppendDbNameConnectionString = "-ReadOnly";
        }
    }
}