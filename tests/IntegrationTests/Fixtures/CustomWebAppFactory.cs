using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebApi;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests.Fixtures
{
    public class CustomWebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private const string ciConnectionStringEnvName = "CI_CONNSTRING";
        protected string AppendDbNameConnectionString { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // NOTE: start reading the setup here, this runs first:

            var root = Directory.GetCurrentDirectory();

            builder
                .UseContentRoot(root)
                .UseEnvironment("IntegrationTest");

            AddIntegrationTestConfig(builder);

            base.ConfigureWebHost(builder);
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);

            // NOTE: do whatever with the host

            var env = host.Services.GetRequiredService<IWebHostEnvironment>();
            var config = host.Services.GetService<IConfiguration>();

            Program.TryRecreateAndMigrateDatabaseAsync(host, env, config).GetAwaiter().GetResult();
            Program.TrySeedDatabaseAsync(host, env, config).GetAwaiter().GetResult();

            return host;
        }

        private void AddIntegrationTestConfig(IWebHostBuilder builder)
        {
            var projectDir = Directory.GetCurrentDirectory();

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(projectDir)
                .AddJsonFile("appsettings.IntegrationTest.json")
                .AddJsonFile("appsettings.IntegrationTest.localhost.json", optional : true)
                .AddEnvironmentVariables();

            var config = TryAppendDatabaseNameToActiveConnectionString(configBuilder);

            builder.ConfigureAppConfiguration(opts => opts.AddConfiguration(config));
        }

        private IConfigurationRoot TryAppendDatabaseNameToActiveConnectionString(IConfigurationBuilder configBuilder)
        {
            TryLoadCiConnectionString(
                configBuilder,
                out var config,
                    out var environmentConnectionString);

            var connectionString = environmentConnectionString ?? config.GetConnectionString("DatabaseConnection");
            var connectionStringBuilder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            connectionStringBuilder["database"] += AppendDbNameConnectionString;

            // NOTE: overrides the active connection string with the one provided below
            configBuilder.AddInMemoryCollection(
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("ConnectionStrings:DatabaseConnection", connectionStringBuilder.ConnectionString)
                });

            return configBuilder.Build();
        }

        /// <summary>
        /// Tries to load CI connection string from environment variable name stored in ciConnectionStringEnvName const.
        /// </summary>
        private void TryLoadCiConnectionString(
            IConfigurationBuilder configBuilder,
            out IConfigurationRoot config,
            out string environmentConnectionString)
        {
            config = configBuilder.Build();
            var ciEnvConnectionStringSection = config.GetSection(ciConnectionStringEnvName);

            environmentConnectionString = null;
            if (ciEnvConnectionStringSection != null)
            {
                environmentConnectionString = ciEnvConnectionStringSection.Value;
            }
        }
    }
}
