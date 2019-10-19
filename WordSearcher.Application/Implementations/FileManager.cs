using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WordSearcher.Application.Contracts;
using WordSearcher.Infrastructure.Contracts;
using WordSearcher.Models;

namespace WordSearcher.Application.Implementations
{
    public class FileManager : IFileManager
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ILogger _logger;
        private readonly IFileProcessor _fileProcessor;

        public FileManager(IConfigurationProvider configurationProvider, ILogger logger, IFileProcessor fileProcessor)
        {
            _configurationProvider = configurationProvider;
            _logger = logger;
            _fileProcessor = fileProcessor;
        }

        public FileProcessed GenerateFileProcessed(string filePath)
        {
            _logger.Info($"Starting to generate a fileProcessed of '{filePath}'");

            var fileProcessed = new FileProcessed
            {
                FilePath = filePath,
                FileName = Path.GetFileName(filePath),
                Dictionary = _fileProcessor.CountWordsOfFile(filePath)
            };

            _logger.Info($"Finish to generate a fileProcessed of '{filePath}' with name={fileProcessed.FileName}");

            return fileProcessed;
        }

        public string[] GetFilePathsToProcess(string folderPath)
        {
            _logger.Info($"Starting to get the file paths to process the folder '{folderPath}'");

            var textExtension = _configurationProvider.GetValueFromAppSettings("textExtension");
            if (string.IsNullOrEmpty(textExtension))
            {
                throw new ArgumentException("TextExtension is a required field, please, fill it at AppSettings.");
            }

            var files = Directory.GetFiles(folderPath, textExtension);

            _logger.Info($"Finished to get the file paths to process the folder '{folderPath}'. Total files to process = {files.Length}");

            return files;
        }

        public List<FileProcessed> ProcessAllFilesFromFolder(string folder)
        {
            _logger.Info($"Starting to process all files of folder '{folder}'");

            var filesToProcess = GetFilePathsToProcess(folder);
            var processedFiles = new List<FileProcessed>();

            for (int i = 0; i < filesToProcess.Length; i++)
            {
                processedFiles.Add(GenerateFileProcessed(filesToProcess[i]));
            }

            _logger.Info($"Finished to process all files of folder '{folder}'. Total files processed = {processedFiles.Count}");

            return processedFiles;
        }

        public List<SearchResult> SearchWord(List<FileProcessed> files, string word)
        {
            var results = new List<SearchResult>();

            foreach (var file in files)
            {
                var result = new SearchResult {FileName = file.FileName, NumberOfTimesFound = 0};
                
                if (file.Dictionary.ContainsKey(word))
                {
                    result.NumberOfTimesFound = file.Dictionary[word];
                }

                results.Add(result);
            }

            return results;
        }

        public List<SearchResult> GetSortedResultsOfSearchingForAWord(List<FileProcessed> files, string word)
        {
            var results = SearchWord(files, word);
            var numberOfResultsToShow = _configurationProvider.GetIntValueFromAppSettings("numberOfResultsToShow");

            results = results.OrderByDescending(r => r.NumberOfTimesFound).Take(numberOfResultsToShow).ToList();

            return results;
        }
    }
}