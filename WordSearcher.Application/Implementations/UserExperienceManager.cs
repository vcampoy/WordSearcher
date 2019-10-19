using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using WordSearcher.Application.Contracts;
using WordSearcher.Infrastructure.Contracts;
using WordSearcher.Models;

namespace WordSearcher.Application.Implementations
{
    public class UserExperienceManager : IUserExperienceManager
    {
        private readonly string _finishWord;
        private readonly ILogger _logger;

        public UserExperienceManager(ILogger logger)
        {
            _logger = logger;
            _finishWord = ConfigurationManager.AppSettings["finishWord"];
        }

        public bool IsFinishWord(string word)
        {
            return word == _finishWord;
        }

        public void DisplayResults(List<SearchResult> processedFiles)
        {
            if (processedFiles.Any(f => f.NumberOfTimesFound > 0))
            {
                foreach (var file in processedFiles)
                {
                    _logger.Info($"{file.FileName}: {file.NumberOfTimesFound}");
                }
            }
            else
            {
                _logger.Info("No results found");
            }
            
        }
    }
}