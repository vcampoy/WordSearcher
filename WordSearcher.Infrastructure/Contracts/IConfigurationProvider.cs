namespace WordSearcher.Infrastructure.Contracts
{
    public interface IConfigurationProvider
    {
        string GetValueFromAppSettings(string key);

        int GetIntValueFromAppSettings(string key);
    }
}
