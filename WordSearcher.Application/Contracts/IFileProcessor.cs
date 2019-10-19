using System.Collections.Generic;

namespace WordSearcher.Application.Contracts
{
    public interface IFileProcessor
    {
        Dictionary<string, int> CountWordsOfFile(string filePath);
    }
}
