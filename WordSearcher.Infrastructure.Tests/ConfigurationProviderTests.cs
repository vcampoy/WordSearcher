using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordSearcher.Infrastructure.Contracts;
using WordSearcher.Infrastructure.Implementations;

namespace WordSearcher.Infrastructure.Tests
{
    [TestClass]
    public class ConfigurationProviderTests
    {
        private IConfigurationProvider _configurationProvider;

        [TestInitialize]
        public void Setup()
        {
            _configurationProvider = new ConfigurationProvider();
        }

        [TestMethod]
        public void Given_GetValueFromAppSettings_getsNonExistingKey_returnsNull()
        {
            Assert.IsNull(_configurationProvider.GetValueFromAppSettings("InventedKey"));
        }

        [TestMethod]
        public void Given_GetValueFromAppSettings_getsExistingKey_returnsValue()
        {
            Assert.IsTrue(_configurationProvider.GetValueFromAppSettings("finishWord") == ":quit");
        }
    }
}
