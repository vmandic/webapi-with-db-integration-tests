using IntegrationTests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataAccess;
using Xunit;

namespace IntegrationTests.HttpRestApi.V1.Modificaton
{
    [Trait("Modification", nameof(WebApi.Controllers.UsersController))]
    public class UserControllerModificationTests : IClassFixture<DbSeededWebAppFactory>
    {
        private readonly HttpClient httpClient;
        private readonly AppDbContext dbContext;

        public UserControllerModificationTests(DbSeededWebAppFactory webAppFactory)
        {
            // NOTE: reuses the database between tests in this test class file

            httpClient = webAppFactory.CreateClient();
            dbContext = (AppDbContext) webAppFactory.Services.GetService(typeof(AppDbContext));
        }

        [Fact]
        public async Task Should_post_user()
        {
            // ARRANGE:
            var expectedUser = new { username = "vmandic-" + Guid.NewGuid().ToString("N") };

            // ACT:
            var apiResponse = await httpClient.PostAsJsonAsync("api/users", expectedUser);

            // ASSERT:
            apiResponse.EnsureSuccessStatusCode();
            Assert.Equal(1, dbContext.Users.Count(x => x.Username == expectedUser.username));
        }
    }
}
