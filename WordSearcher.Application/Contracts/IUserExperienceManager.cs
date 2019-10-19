using System.Collections.Generic;
using WordSearcher.Models;

namespace WordSearcher.Application.Contracts
{
    public interface IUserExperienceManager
    {
        bool IsFinishWord(string word);

        void DisplayResults(List<SearchResult> processedFiles);
    }
}
