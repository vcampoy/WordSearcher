namespace WordSearcher.Infrastructure.Contracts
{
    public interface IConfigurationProvider
    {
        string GetValueFromAppSettings(string key);
    }
}
