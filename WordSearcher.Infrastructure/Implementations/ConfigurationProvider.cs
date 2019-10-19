using System;
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

        public int GetIntValueFromAppSettings(string key)
        {
            var value = ConfigurationManager.AppSettings[key];

            if(!int.TryParse(value, out var number))
            {
                throw new ArgumentException($"The key {key} is not a integer, please check it at AppSettings");
            }

            return number;
        }
    }
}
