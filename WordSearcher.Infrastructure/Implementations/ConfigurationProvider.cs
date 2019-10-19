using System.Configuration;
using WordSearcher.Infrastructure.Contracts;

namespace WordSearcher.Infrastructure.Implementations
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string GetValueFromAppSettings(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            return value;
        }
    }
}
