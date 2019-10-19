using log4net;
using WordSearcher.Infrastructure.Contracts;

namespace WordSearcher.Infrastructure.Implementations
{
    public class Logger : ILogger
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(Logger));

        public void Info(string text)
        {
            _log.Info(text);
        }

        public void Warn(string text)
        {
            _log.Warn(text);
        }

        public void Error(string text)
        {
            _log.Error(text);
        }
    }
}
