using System.Configuration;
using WordSearcher.Application.Contracts;

namespace WordSearcher.Application.Implementations
{
    public class UserExperienceManager : IUserExperienceManager
    {
        private readonly string _finishWord;

        public UserExperienceManager()
        {
            _finishWord = ConfigurationManager.AppSettings["finishWord"];
        }

        public bool IsFinishWord(string word)
        {
            return word == _finishWord;
        }
    }
}