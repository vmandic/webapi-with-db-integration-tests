using Microsoft.Extensions.Configuration;

namespace WebApi.Helpers
{
    public class AppHelpers
    {
        internal static T GetConfig<T>(IConfiguration config)
        {
            var sectionName = typeof(T).Name;
            var section = config.GetSection(sectionName);
            
            return section.Get<T>();
        }
    }
}