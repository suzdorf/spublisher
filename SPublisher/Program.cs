using System.IO;
using System.Linq;
using SPublisher.BuildExecutor;
using SPublisher.Configuration;
using SPublisher.Core;
using SPublisher.IisManagement;

namespace SPublisher
{
    class Program
    {
        private static readonly ILogger Logger = new Logger();
        // IIS Management
        private static readonly ServerManagerAccessor ServerManagerAccessor = new ServerManagerAccessor();
        private static readonly IServerManagerDataProvider ServerManagerDataProvider =  new ServerManagerDataProvider(ServerManagerAccessor);
        private static readonly IApplicationCreator ApplicationCreator = new ApplicationCreator(ServerManagerDataProvider);
        private static readonly ISiteCreator SiteCreator = new SiteCreator(ServerManagerAccessor, ApplicationCreator);

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

            if (model.Applications.Any())
            {
                Logger.LogEvent(SPublisherEvent.IisManagementStarted);
                //SiteCreator.Create(model.Applications);
                Logger.LogEvent(SPublisherEvent.IisManagementCompleted);
            }

            Logger.LogEvent(SPublisherEvent.SPublisherCompleted);
        }
    }
}
