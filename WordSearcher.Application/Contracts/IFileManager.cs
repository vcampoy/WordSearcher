using System.Collections.Generic;
using WordSearcher.Models;

namespace WordSearcher.Application.Contracts
{
    public interface IFileManager
    {
        FileProcessed GenerateFileProcessed(string filePath);

        string[] GetFilePathsToProcess(string folderPath);

        List<FileProcessed> ProcessAllFilesFromFolder(string folder);

        List<SearchResult> SearchWord(List<FileProcessed> files, string word);

        List<SearchResult> GetSortedResultsOfSearchingForAWord(List<FileProcessed> files, string word);
    }
}
