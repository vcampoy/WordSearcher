using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordSearcher.Application.Contracts;
using WordSearcher.Application.Implementations;
using WordSearcher.Infrastructure.Contracts;

namespace WordSearcher.Application.Tests
{
    [TestClass]
    public class FileProcessorTests
    {
        private IFileProcessor _fileProcessor;
        private IConfigurationProvider _fakeConfigurationProvider;

        [TestInitialize]
        public void Setup()
        {
            _fakeConfigurationProvider = A.Fake<IConfigurationProvider>();

            _fileProcessor = new FileProcessor(_fakeConfigurationProvider);
        }

        #region GetPunctuationMarks

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "PunctuationMarks is a required field, please, fill it at AppSettings.")]
        public void Given_GetPunctuationMarks_getsNotFilledPunctuationMarksOnAppSettings_throwsException()
        {
            A.CallTo(() => _fakeConfigurationProvider.GetValueFromAppSettings("punctuationMarks")).Returns(null);
            _fileProcessor.GetPunctuationMarks();
        }

        [TestMethod]
        public void Given_GetPunctuationMarks_getsOnePunctuationMarkOnAppSettings_()
        {
            A.CallTo(() => _fakeConfigurationProvider.GetValueFromAppSettings("punctuationMarks")).Returns(",");
            var response = _fileProcessor.GetPunctuationMarks();
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count == 1);
            Assert.IsTrue(response.Any(r=>r == ','));
        }

        [TestMethod]
        public void Given_GetPunctuationMarks_getsThreePunctuationMarkOnAppSettings_()
        {
            A.CallTo(() => _fakeConfigurationProvider.GetValueFromAppSettings("punctuationMarks")).Returns(",.;");
            var response = _fileProcessor.GetPunctuationMarks();
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count == 3);
            Assert.IsTrue(response.Any(r => r == ','));
            Assert.IsTrue(response.Any(r => r == '.'));
            Assert.IsTrue(response.Any(r => r == ';'));
        }

        #endregion

    }
}
