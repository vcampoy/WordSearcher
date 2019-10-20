using System;
using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordSearcher.Application.Contracts;
using WordSearcher.Application.Implementations;
using WordSearcher.Infrastructure.Contracts;

namespace WordSearcher.Application.Tests.Implementations
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

        #region CleanTextFromPunctuationMarks

        [TestMethod]
        public void Given_CleanTextFromPunctuationMarks_getsNullText_returnsNullText()
        {
            Assert.IsNull(_fileProcessor.CleanTextFromPunctuationMarks(null));
        }

        [TestMethod]
        public void Given_CleanTextFromPunctuationMarks_getsEmptyText_returnsEmptyText()
        {
            Assert.IsTrue(_fileProcessor.CleanTextFromPunctuationMarks(string.Empty) == string.Empty);
        }

        [TestMethod]
        public void Given_CleanTextFromPunctuationMarks_getsOnlyPunctuationMarksText_returnsOnlySpacesText()
        {
            A.CallTo(() => _fakeConfigurationProvider.GetValueFromAppSettings("punctuationMarks")).Returns(",.;");
            Assert.IsTrue(_fileProcessor.CleanTextFromPunctuationMarks(",.;.") == "    ");
        }
        
        [TestMethod]
        public void Given_CleanTextFromPunctuationMarks_getsRegularText_returnsCleanText()
        {
            A.CallTo(() => _fakeConfigurationProvider.GetValueFromAppSettings("punctuationMarks")).Returns(",.;");
            Assert.IsTrue(_fileProcessor.CleanTextFromPunctuationMarks("This is an example, but it can be whatever. True story;") == "This is an example  but it can be whatever  True story ");
        }

        #endregion

        #region GetLowerCaseWord

        [TestMethod]
        public void Given_GetLowerCaseWord_getsNullWord_returnsNullWord()
        {
            Assert.IsNull(_fileProcessor.GetLowerCaseWord(null));
        }

        [TestMethod]
        public void Given_GetLowerCaseWord_getsEmptyWord_returnsEmptyWord()
        {
            Assert.IsTrue(_fileProcessor.GetLowerCaseWord(string.Empty) == string.Empty);
        }

        [TestMethod]
        public void Given_GetLowerCaseWord_getslowerCaseWord_returnsLowerCaseWord()
        {
            Assert.IsTrue(_fileProcessor.GetLowerCaseWord("word") == "word");
        }

        [TestMethod]
        public void Given_GetLowerCaseWord_getsRegularWord_returnsLowerCaseWord()
        {
            Assert.IsTrue(_fileProcessor.GetLowerCaseWord("WoRd") == "word");
        }


        [TestMethod]
        public void Given_GetLowerCaseWord_getsWordWithNumbers_returnsLowerCaseWordWithNumbers()
        {
            Assert.IsTrue(_fileProcessor.GetLowerCaseWord("w0rD") == "w0rd");
        }

        #endregion

        #region GenerateDictionary

        [TestMethod]
        public void Given_GenerateDictionary_getsNullText_returnsEmptyDictionary()
        {
            var result = _fileProcessor.GenerateDictionary(null);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void Given_GenerateDictionary_getsEmptyText_returnsEmptyDictionary()
        {
            var result = _fileProcessor.GenerateDictionary(string.Empty);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void Given_GenerateDictionary_getsASingleWordText_returnsDictionaryWithOneElement()
        {
            var result = _fileProcessor.GenerateDictionary("word");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.ContainsKey("word"));
            Assert.IsTrue(result["word"] == 1);
        }

        [TestMethod]
        public void Given_GenerateDictionary_getsATwoWordsText_returnsDictionaryWithTwoElements()
        {
            var result = _fileProcessor.GenerateDictionary("word new");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.ContainsKey("word"));
            Assert.IsTrue(result["word"] == 1);
            Assert.IsTrue(result.ContainsKey("new"));
            Assert.IsTrue(result["new"] == 1);
        }

        [TestMethod]
        public void Given_GenerateDictionary_getsAComplexText_returnsDictionary()
        {
            var result = _fileProcessor.GenerateDictionary("This is a phrase that I need to save in a dictionary this this this");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 11);
            Assert.IsTrue(result.ContainsKey("this"));
            Assert.IsTrue(result["this"] == 4);
            Assert.IsTrue(result.ContainsKey("is"));
            Assert.IsTrue(result["is"] == 1);
            Assert.IsTrue(result.ContainsKey("a"));
            Assert.IsTrue(result["a"] == 2);
            Assert.IsTrue(result.ContainsKey("phrase"));
            Assert.IsTrue(result["phrase"] == 1);
            Assert.IsTrue(result.ContainsKey("that"));
            Assert.IsTrue(result["that"] == 1);
            Assert.IsTrue(result.ContainsKey("i"));
            Assert.IsTrue(result["i"] == 1);
            Assert.IsTrue(result.ContainsKey("need"));
            Assert.IsTrue(result["need"] == 1);
            Assert.IsTrue(result.ContainsKey("to"));
            Assert.IsTrue(result["to"] == 1);
            Assert.IsTrue(result.ContainsKey("save"));
            Assert.IsTrue(result["save"] == 1);
            Assert.IsTrue(result.ContainsKey("in"));
            Assert.IsTrue(result["in"] == 1);
            Assert.IsTrue(result.ContainsKey("dictionary"));
            Assert.IsTrue(result["dictionary"] == 1);
            Assert.IsTrue(!result.ContainsKey("This"));
            Assert.IsTrue(!result.ContainsKey("I"));
        }

        #endregion

        #region CountWords

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Filepath is a required field, please, fill it.")]
        public void Given_CountWordsOfFile_throwsExceptionWithNullPath()
        {
            _fileProcessor.CountWordsOfFile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Filepath is a required field, please, fill it.")]
        public void Given_CountWordsOfFile_throwsExceptionWithEmptyPath()
        {
            _fileProcessor.CountWordsOfFile(string.Empty);
        }

        [TestMethod]
        public void Given_CountWordsOfFile_getsGoodPath_returnsDictionary()
        {
            //This is a sample test to pass the Unit Testing.
            var path = "SampleText.txt";

            A.CallTo(() => _fakeConfigurationProvider.GetValueFromAppSettings("punctuationMarks")).Returns(",.;");

            var result = _fileProcessor.CountWordsOfFile(path);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 11);
            Assert.IsTrue(result.ContainsKey("this"));
            Assert.IsTrue(result["this"] == 4);
            Assert.IsTrue(result.ContainsKey("is"));
            Assert.IsTrue(result["is"] == 1);
            Assert.IsTrue(result.ContainsKey("a"));
            Assert.IsTrue(result["a"] == 2);
            Assert.IsTrue(result.ContainsKey("phrase"));
            Assert.IsTrue(result["phrase"] == 1);
            Assert.IsTrue(result.ContainsKey("that"));
            Assert.IsTrue(result["that"] == 1);
            Assert.IsTrue(result.ContainsKey("i"));
            Assert.IsTrue(result["i"] == 1);
            Assert.IsTrue(result.ContainsKey("need"));
            Assert.IsTrue(result["need"] == 1);
            Assert.IsTrue(result.ContainsKey("to"));
            Assert.IsTrue(result["to"] == 1);
            Assert.IsTrue(result.ContainsKey("save"));
            Assert.IsTrue(result["save"] == 1);
            Assert.IsTrue(result.ContainsKey("in"));
            Assert.IsTrue(result["in"] == 1);
            Assert.IsTrue(result.ContainsKey("dictionary"));
            Assert.IsTrue(result["dictionary"] == 1);
            Assert.IsTrue(!result.ContainsKey("This"));
            Assert.IsTrue(!result.ContainsKey("I"));
        }

        #endregion
    }
}
