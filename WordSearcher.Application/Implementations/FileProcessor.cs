using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WordSearcher.Application.Contracts;
using WordSearcher.Infrastructure.Contracts;

namespace WordSearcher.Application.Implementations
{
    public class FileProcessor : IFileProcessor
    {
        private readonly IConfigurationProvider _configurationProvider;

        public FileProcessor(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public Dictionary<string, int> CountWordsOfFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException($"Filepath is a required field, please, fill it.");
            }

            var text = File.ReadAllText(filePath);

            text = CleanTextFromPunctuationMarks(text);

            var dictionary = GenerateDictionary(text);

            return dictionary;
        }
        
        public string GetLowerCaseWord(string word)
        {
            return string.IsNullOrEmpty(word) ? word : word.ToLower();
        }

        public Dictionary<string, int> GenerateDictionary(string text)
        {
            var dictionary = new Dictionary<string, int>();

            if (string.IsNullOrEmpty(text))
            {
                return dictionary;
            }

            var words = text.Split(' ');
            
            //Tip: I prefer to use a for-loop in places where the performance is required.
            //In this case, it is probably that we are going to process a lot of words, so for-loop is better than a foreach-loop
            for (int i = 0; i < words.Length; i++)
            {
                var lowerCaseWord = GetLowerCaseWord(words[i]);

                if (lowerCaseWord == "")
                {
                    continue;
                }

                if (dictionary.ContainsKey(lowerCaseWord))
                {
                    dictionary[lowerCaseWord]++;
                }
                else
                {
                    dictionary.Add(lowerCaseWord, 1);
                }
            }

            return dictionary;
        }

        public string CleanTextFromPunctuationMarks(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var punctuationMarks = GetPunctuationMarks();

            foreach (var mark in punctuationMarks)
            {
                text = text.Replace(mark.ToString(), " ");
            }

            return text;
        }

        public List<char> GetPunctuationMarks()
        {
            var punctuationMarks = _configurationProvider.GetValueFromAppSettings("punctuationMarks");
            if (string.IsNullOrEmpty(punctuationMarks))
            {
                throw new ArgumentException($"PunctuationMarks is a required field, please, fill it at AppSettings.");
            }

            return punctuationMarks.ToList();
        }
    }
}
