using IntegrationTests.Fixtures;
using IntegrationTests.Fixtures.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataAccess;
using WebApi.DataAccess.Entities;
using Xunit;

namespace IntegrationTests.HttpRestApi.V1.ReadOnly
{
    [Collection(nameof(ReadOnlyDbSeededCollectionWebAppFactory))]
    [Trait("ReadOnly", nameof(WebApi.Controllers.UsersController))]
    public class UserControllerReadOnlyTests
    {
        private readonly HttpClient httpClient;
        private readonly AppDbContext dbContext;

        public UserControllerReadOnlyTests(ReadOnlyDbSeededWebAppFactory webAppFactory)
        {
            // NOTE: reuses the database between tests and all class test files that use [Collection(nameof(ReadOnlyDbSeededCollectionWebAppFactory))]

            httpClient = webAppFactory.CreateClient();
            dbContext = (AppDbContext)webAppFactory.Services.GetService(typeof(AppDbContext));
        }

        [Fact]
        public async Task Should_return_seeded_five_users()
        {
            // ACT:
            var apiResponse = await httpClient.GetAsync("api/users");

            // ASERT:
            apiResponse.EnsureSuccessStatusCode();
            var responseModel = await apiResponse.Content.ReadAsAsync<IReadOnlyCollection<User>>();

            Assert.NotNull(responseModel);
            Assert.Equal(5, responseModel.Count);
            Assert.DoesNotContain(responseModel, x => x.Username.StartsWith("vmandic-"));
            Assert.Equal(1, responseModel.Min(x => x.Id));
            Assert.Equal(5, responseModel.Max(x => x.Id));
        }
    }
}
