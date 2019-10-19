using System;
using Unity;
using WordSearcher.Application.Contracts;
using WordSearcher.Application.Implementations;
using WordSearcher.Infrastructure.Contracts;
using WordSearcher.Infrastructure.Implementations;

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

                var folder = args[0];
                
                logger.Info($"Start to process files of folder='{folder}'");

                //TODO: Add processing files method here

                var processedFiles = 0;
                logger.Info($"Finish to process {processedFiles} files of folder='{folder}'");

                var word = string.Empty;

                var userExpecienceManager = IoCHelper.GetContainer().Resolve<IUserExperienceManager>();

                do 
                {
                    System.Console.WriteLine("Please, enter a word to search (write ':quit' to finish)");
                    
                    word = System.Console.ReadLine();
                    
                    if (!userExpecienceManager.IsFinishWord(word))
                    {
                        //TODO: Add Word search algorithm here
                    }

                } while (!userExpecienceManager.IsFinishWord(word));

                logger.Info("Finished WordSearcher");
            }
            catch (Exception ex)
            {
                logger.Error($"Error on WordSearcher.Console. ERROR={ex.Message}. StackTracer={ex.StackTrace}");

                System.Console.WriteLine("Press any key to finish.");
                System.Console.ReadLine();
            }
        }

        #region Dependencies registration

        private static void RegisterDependencies()
        {
            var container = IoCHelper.GetContainer();

            RegisterApplicationDependencies(container);
            RegisterInfrastructureDependencies(container);
        }

        private static void RegisterApplicationDependencies(IUnityContainer container)
        {
            container.RegisterType<IUserExperienceManager, UserExperienceManager>();
            container.RegisterType<IFileProcessor, FileProcessor>();
        }

        private static void RegisterInfrastructureDependencies(IUnityContainer container)
        {
            container.RegisterType<ILogger, Logger>();
            container.RegisterType<IConfigurationProvider, ConfigurationProvider>();
        }

        #endregion
    }
}
