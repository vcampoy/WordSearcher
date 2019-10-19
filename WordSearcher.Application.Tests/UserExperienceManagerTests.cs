using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordSearcher.Application.Contracts;
using WordSearcher.Application.Implementations;

namespace WordSearcher.Application.Tests
{
    [TestClass]
    public class UserExperienceManagerTests
    {
        private IUserExperienceManager _userExperienceManager;

        [TestInitialize]
        public void Setup()
        {
            _userExperienceManager = new UserExperienceManager();
        }

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
    }
}
