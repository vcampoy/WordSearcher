using log4net;

namespace WordSearcher.Infrastructure.Implementation
{
    public class Logger : ILogger
    {
        //public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ILog _log = LogManager.GetLogger(typeof(Logger));

        public void Info(string text)
        {
            _log.Info(text);
        }

        public void Error(string text)
        {
            _log.Error(text);
        }
    }

    public interface ILogger
    {
        void Info(string text);
        void Error(string text);
    }
}
