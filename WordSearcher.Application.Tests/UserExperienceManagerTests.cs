using System.Collections.Generic;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordSearcher.Application.Contracts;
using WordSearcher.Application.Implementations;
using WordSearcher.Infrastructure.Contracts;
using WordSearcher.Models;

namespace WordSearcher.Application.Tests
{
    [TestClass]
    public class UserExperienceManagerTests
    {
        private IUserExperienceManager _userExperienceManager;
        private ILogger _logger;

        [TestInitialize]
        public void Setup()
        {
            _logger = A.Fake<ILogger>();
            _userExperienceManager = new UserExperienceManager(_logger);
        }

        #region IsFinishWord

        [TestMethod]
        public void Given_IsFinishWord_getsNull_returnsFalse()
        {
            Assert.IsFalse(_userExperienceManager.IsFinishWord(null));
        }

        [TestMethod]
        public void Given_IsFinishWord_getsEmptyString_returnsFalse()
        {
            Assert.IsFalse(_userExperienceManager.IsFinishWord(string.Empty));
        }

        [TestMethod]
        public void Given_IsFinishWord_getsWrongString_returnsFalse()
        {
            Assert.IsFalse(_userExperienceManager.IsFinishWord("wrong"));
        }

        [TestMethod]
        public void Given_IsFinishWord_getsGoodString_returnsTrue()
        {
            Assert.IsTrue(_userExperienceManager.IsFinishWord(":quit"));
        }

        #endregion

        #region DisplayResults

        [TestMethod]
        public void Given_DisplayResults_getsNullFiles_logsNoResults()
        {
            _userExperienceManager.DisplayResults(null);
            A.CallTo(() => _logger.Info("No results found")).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public void Given_DisplayResults_getsEmptyFiles_logsNoResults()
        {
            _userExperienceManager.DisplayResults(new List<SearchResult>());
            A.CallTo(() => _logger.Info("No results found")).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public void Given_DisplayResults_getsFilesWithoutResults_logsNoResults()
        {
            var results = new List<SearchResult>
            {
                new SearchResult {FileName = "fileName", NumberOfTimesFound = 0}
            };

            _userExperienceManager.DisplayResults(results);
            
            A.CallTo(() => _logger.Info("No results found")).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public void Given_DisplayResults_getsFilesWithResults_logsResults()
        {
            var results = new List<SearchResult>
            {
                new SearchResult {FileName = "fileName", NumberOfTimesFound = 5},
                new SearchResult {FileName = "fileName2", NumberOfTimesFound = 4},
                new SearchResult {FileName = "fileName3", NumberOfTimesFound = 1}
            };
            
            _userExperienceManager.DisplayResults(results);
            
            A.CallTo(() => _logger.Info("fileName: 5")).MustHaveHappenedOnceExactly();
            A.CallTo(() => _logger.Info("fileName2: 4")).MustHaveHappenedOnceExactly();
            A.CallTo(() => _logger.Info("fileName3: 1")).MustHaveHappenedOnceExactly();
        }

        #endregion
    }
}
