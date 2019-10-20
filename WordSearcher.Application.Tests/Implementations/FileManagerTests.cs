using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordSearcher.Application.Contracts;
using WordSearcher.Application.Implementations;
using WordSearcher.Infrastructure.Contracts;
using WordSearcher.Models;

namespace WordSearcher.Application.Tests.Implementations
{
    [TestClass]
    public  class FileManagerTests
    {
        private IFileManager _fileManager;
        private IFileProcessor _fakeFileProcessor;
        private IConfigurationProvider _fakeConfigurationProvider;
        private ILogger _fakeLogger;

        private Dictionary<string, int> _dictionary;
        private List<FileProcessed> _filesProcessed;

        [TestInitialize]
        public void Setup()
        {
            _fakeFileProcessor = A.Fake<IFileProcessor>();
            _fakeConfigurationProvider = A.Fake<IConfigurationProvider>();
            _fakeLogger = A.Fake<ILogger>();

            _fileManager = new FileManager(_fakeConfigurationProvider, _fakeLogger, _fakeFileProcessor);

            _dictionary = new Dictionary<string, int>
            {
                { "this", 1 },
                { "is", 2 },
                { "a", 3 },
                { "test", 1 }
            };

            _filesProcessed = new List<FileProcessed>
            {
                new FileProcessed
                {
                    FileName = "fileName.txt",
                    FilePath = "folder\\fileName.txt",
                    Dictionary = new Dictionary<string, int>
                    {
                        { "word", 1 },
                        { "text", 3 },
                        { "string", 5 },
                        { "gol", 1 }
                    }
                },
                new FileProcessed
                {
                    FileName = "fileName2.txt",
                    FilePath = "folder\\fileName2.txt",
                    Dictionary = new Dictionary<string, int>
                    {
                        { "word", 2 },
                        { "text", 2 },
                        { "string", 4 }
                    }
                }
            };
        }

        #region GenerateFileProcessed

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "FilePath is a required field, please, fill it.")]
        public void Given_GenerateFileProcessed_getsNullFilePath_throwsException()
        {
            _fileManager.GenerateFileProcessed(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "FilePath is a required field, please, fill it.")]
        public void Given_GenerateFileProcessed_getsEmptyFilePath_throwsException()
        {
            _fileManager.GenerateFileProcessed(string.Empty);
        }

        [TestMethod]
        public void Given_GenerateFileProcessed_getsFilePath_returnsAFileProcessed()
        {
            var filePath = "C:\\folder\\filePath.txt";
            var fileName = "filePath.txt";

            A.CallTo(() => _fakeFileProcessor.CountWordsOfFile(filePath)).Returns(_dictionary);
            var response = _fileManager.GenerateFileProcessed(filePath);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.FileName == fileName);
            Assert.IsTrue(response.FilePath == filePath);
            Assert.IsTrue(response.Dictionary.Count == 4);
            Assert.IsTrue(response.Dictionary.ContainsKey("this"));
            Assert.IsTrue(response.Dictionary["this"] == 1);
            Assert.IsTrue(response.Dictionary.ContainsKey("is"));
            Assert.IsTrue(response.Dictionary["is"] == 2);
            Assert.IsTrue(response.Dictionary.ContainsKey("a"));
            Assert.IsTrue(response.Dictionary["a"] == 3);
            Assert.IsTrue(response.Dictionary.ContainsKey("test"));
            Assert.IsTrue(response.Dictionary["test"] == 1);
        }

        #endregion

        #region GetFilePathsToProcess

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "TextExtension is a required field, please, fill it at AppSettings.")]
        public void Given_GetFilePathsToProcess_getsNullTextExtension_throwsException()
        {
            var folderPath = "folderPath";

            A.CallTo(() => _fakeConfigurationProvider.GetValueFromAppSettings("textExtension")).Returns(null);
            _fileManager.GetFilePathsToProcess(folderPath);
        }

        [TestMethod]
        public void Given_GetFilePathsToProcess_getsRegularFolder_returnsFilePaths()
        {
            var folderPath = "SampleFolder";

            A.CallTo(() => _fakeConfigurationProvider.GetValueFromAppSettings("textExtension")).Returns("*.txt");
            var response = _fileManager.GetFilePathsToProcess(folderPath);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Length == 2);
            Assert.IsTrue(response.Contains("SampleFolder\\SampleText.txt"));
            Assert.IsTrue(response.Contains("SampleFolder\\SampleText2.txt"));
        }

        #endregion

        #region ProcessAllFilesFromFolder

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Folder is a required field, please, fill it.")]
        public void Given_ProcessAllFilesFromFolder_getsNullFolder_throwsException()
        {
            _fileManager.ProcessAllFilesFromFolder(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Folder is a required field, please, fill it.")]
        public void Given_ProcessAllFilesFromFolder_getsEmptyFolder_throwsException()
        {
            _fileManager.ProcessAllFilesFromFolder(string.Empty);
        }

        [TestMethod]
        public void Given_ProcessAllFilesFromFolder_getsRegularFolderWithOneFile_returnsOneFileProcessed()
        {
            var folder = "SingleSampleFolder";
            A.CallTo(() => _fakeConfigurationProvider.GetValueFromAppSettings("textExtension")).Returns("*.txt");

            var processed = _fileManager.ProcessAllFilesFromFolder(folder);

            Assert.IsNotNull(processed);
            Assert.IsTrue(processed.Count == 1);
            Assert.IsTrue(processed.Any(p=>p.FileName == "SampleText.txt"));
            Assert.IsTrue(processed.Any(p => p.FilePath == "SingleSampleFolder\\SampleText.txt"));

            //I do not check the content of the dictionary because it has been tested several times in other places
        }

        [TestMethod]
        public void Given_ProcessAllFilesFromFolder_getsRegularFolderWithTwoFiles_returnsTwoFileProcessed()
        {
            var folder = "SampleFolder";
            A.CallTo(() => _fakeConfigurationProvider.GetValueFromAppSettings("textExtension")).Returns("*.txt");

            var processed = _fileManager.ProcessAllFilesFromFolder(folder);

            Assert.IsNotNull(processed);
            Assert.IsTrue(processed.Count == 2);
            Assert.IsTrue(processed.Any(p => p.FileName == "SampleText.txt"));
            Assert.IsTrue(processed.Any(p => p.FilePath == "SampleFolder\\SampleText.txt"));
            Assert.IsTrue(processed.Any(p => p.FileName == "SampleText2.txt"));
            Assert.IsTrue(processed.Any(p => p.FilePath == "SampleFolder\\SampleText2.txt"));

            //I do not check the content of the dictionary because it has been tested several times in other places
        }

        #endregion

        #region SearchWord

        [TestMethod]
        public void Given_SearchWord_getsNullFiles_returnsEmptyList()
        {
            var response = _fileManager.SearchWord(null, "word");

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Any());
        }

        [TestMethod]
        public void Given_SearchWord_getsNoFiles_returnsEmptyList()
        {
            var response = _fileManager.SearchWord(new List<FileProcessed>(), "word");

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "It is required to introduce a word to search.")]
        public void Given_SearchWord_getsNullWord_throwsException()
        {
            _fileManager.SearchWord(_filesProcessed, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "It is required to introduce a word to search.")]
        public void Given_SearchWord_getsEmptyWord_throwsException()
        {
            _fileManager.SearchWord(_filesProcessed, string.Empty);
        }

        [TestMethod]
        public void Given_SearchWord_getsFilesWithAWordOnlyInOne_returnsAListWithOnlyOneMatch()
        {
            var word = "gol";

            A.CallTo(() => _fakeFileProcessor.GetLowerCaseWord(word)).Returns(word);
            
            var response = _fileManager.SearchWord(_filesProcessed, word);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count == 2);
            Assert.IsTrue(response.Any(r=>r.FileName == "fileName.txt" && r.NumberOfTimesFound == 1));
            Assert.IsTrue(response.Any(r => r.FileName == "fileName2.txt" && r.NumberOfTimesFound == 0));
        }

        [TestMethod]
        public void Given_SearchWord_getsFiles_returnsAListWithTwoMatchs()
        {
            var word = "word";

            A.CallTo(() => _fakeFileProcessor.GetLowerCaseWord(word)).Returns(word);

            var response = _fileManager.SearchWord(_filesProcessed, word);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count == 2);
            Assert.IsTrue(response.Any(r => r.FileName == "fileName.txt" && r.NumberOfTimesFound == 1));
            Assert.IsTrue(response.Any(r => r.FileName == "fileName2.txt" && r.NumberOfTimesFound == 2));
        }

        #endregion
    }
}
