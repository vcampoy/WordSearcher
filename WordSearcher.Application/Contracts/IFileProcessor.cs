using System.Collections.Generic;

namespace WordSearcher.Application.Contracts
{
    public interface IFileProcessor
    {
        Dictionary<string, int> CountWordsOfFile(string filePath);

        Dictionary<string, int> GenerateDictionary(string text);

        string CleanTextFromPunctuationMarks(string text);

        List<char> GetPunctuationMarks();

        string GetLowerCaseWord(string word);
    }
}
