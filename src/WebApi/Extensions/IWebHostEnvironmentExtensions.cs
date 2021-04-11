using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebApi.Extensions
{
    public static class IWebHostEnvironmentExtensions
    {
        public static bool IsIntegrationTest(this IWebHostEnvironment environment)
        {
            return environment.IsEnvironment("IntegrationTest");
        }
    }
}