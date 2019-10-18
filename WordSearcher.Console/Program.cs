using System;
using Unity;
using Unity.log4net;
using WordSearcher.Infrastructure.Implementation;

namespace WordSearcher.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            RegisterDependencies();
            log4net.Config.XmlConfigurator.Configure();

            var logger = IoCHelper.GetContainer().Resolve<ILogger>();

            try
            {
                logger.Info("Started WordSearcher");
            }
            catch (Exception ex)
            {
                logger.Error($"Error on WordSearcher.Console. ERROR={ex.Message}. StackTracer={ex.StackTrace}");
            }

            System.Console.WriteLine("Press any key to finish.");
            System.Console.ReadLine();
        }

        private static void RegisterDependencies()
        {
            var container = IoCHelper.GetContainer();
            
            container.AddNewExtension<Log4NetExtension>();

            container.RegisterType<ILogger, Logger>();
        }
    }
}
