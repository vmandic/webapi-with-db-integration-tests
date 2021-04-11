
# WebApi .NET5 + Integration tests with DB migration and seed

This is a sample project demonstrating how to run integration tests against a .NET 5 REST HTTP API by recreating, migrating and seeding a DB with EF ORM included.

This is only bootstrapping sample on which you can build on.

## How to run

Execute `dotnet run` in src\WebApi.

## How to run tests

Execute `dotnet test` in tests\IntegrationTests.

## Caveates

You should not organize projects bundled all up in one single .csproj, rather consider extracting the DAL project to a separate one for more control over your source and for better testability.

Author: Vedran Mandić
