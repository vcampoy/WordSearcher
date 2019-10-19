using System;
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

        #region GetValueFromAppSettings

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

        #endregion

        #region GetIntValueFromAppSettings

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The key inventedKey is not a integer, please check it at AppSettings")]
        public void Given_GetIntValueFromAppSettings_getsNonExistingKey_throwsException()
        {
            _configurationProvider.GetIntValueFromAppSettings("inventedKey");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The key inventedKey is not a integer, please check it at AppSettings")]
        public void Given_GetIntValueFromAppSettings_getsStringKey_throwsException()
        {
            _configurationProvider.GetIntValueFromAppSettings("stringKey");
        }

        [TestMethod]
        public void Given_GetIntValueFromAppSettings_getsIntKey_returnsValue()
        {
            var result = _configurationProvider.GetIntValueFromAppSettings("numberOfResultsToShow");
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 3);
        }

        #endregion
    }
}
