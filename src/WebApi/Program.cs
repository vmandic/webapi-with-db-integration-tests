using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApi.DataAccess;
using WebApi.Extensions;

namespace WebApi
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var env = host.Services.GetRequiredService<IWebHostEnvironment>();
            var config = host.Services.GetService<IConfiguration>();

            await TryRecreateAndMigrateDatabaseAsync(host, env, config);
            await TrySeedDatabaseAsync(host, env, config);

            await host.RunAsync();
        }

        public static async Task TrySeedDatabaseAsync(IHost host, IWebHostEnvironment env, IConfiguration config)
        {
            var shouldSeedDatabase = config.GetValue<bool>("ShouldSeedDatabase");

            if (env.IsIntegrationTest() && shouldSeedDatabase)
            {
                Console.WriteLine("Executing SQL seed script...");

                var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
                using var scope = scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var sqlScriptContents = await File.ReadAllTextAsync(Path.Combine("DataAccess", "Seed", "seed-db.sql"));
                _ = await dbContext.Database.ExecuteSqlRawAsync(sqlScriptContents);

                Console.WriteLine("SQL seed script executed");
            }
        }

        public static async Task TryRecreateAndMigrateDatabaseAsync(IHost host, IWebHostEnvironment env, IConfiguration config)
        {
            var shouldRecreateAndMigrateDatabase = config.GetValue<bool>("ShouldRecreateAndMigrateDatabase");

            if (env.IsIntegrationTest() && shouldRecreateAndMigrateDatabase)
            {
                Console.WriteLine("Executing database recreate and EF migrate...");

                var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
                using var scope = scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                _ = await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.MigrateAsync();

                Console.WriteLine("Database recreated and EF migrated");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
