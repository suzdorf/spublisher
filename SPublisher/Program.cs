using System.IO;
using System.Linq;
using SPublisher.BuildExecutor;
using SPublisher.Configuration;
using SPublisher.Core;

namespace SPublisher
{
    class Program
    {
        private static readonly ILogger Logger = new Logger();
        // Build executor
        private static readonly IBuildStepExecutorFactory BuildStepExecutorFactory =  new BuildStepExecutorFactory(BuildStepConfiguration.BuildStepExecutors);
        private static readonly IBuildExecutor BuildExecutor = new BuildExecutor.BuildExecutor(BuildStepExecutorFactory, Logger);

        // Configuration
        private static readonly IConfigurationFactory ConfigurationFactory = new ConfigurationFactory(BuildStepConfiguration.BuildStepModelCreators);
        static void Main(string[] args)
        {
            Logger.LogEvent(SPublisherEvent.SPublisherStarted);
            var json = File.ReadAllText("spublisher.json");
            var model = ConfigurationFactory.Get(json);

            if (model.BuildSteps.Any())
            {
                Logger.LogEvent(SPublisherEvent.BuildExecutionStarted);
                BuildExecutor.Execute(model.BuildSteps);
                Logger.LogEvent(SPublisherEvent.BuildExecutionCompleted);
            }

            Logger.LogEvent(SPublisherEvent.SPublisherCompleted);
        }
    }
}
