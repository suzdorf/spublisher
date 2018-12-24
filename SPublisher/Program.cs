using System;
using System.Linq;
using System.Security.Principal;
using SPublisher.BuildExecutor;
using SPublisher.Configuration;
using SPublisher.Configuration.Exceptions;
using SPublisher.Core;
using SPublisher.Core.Exceptions;

namespace SPublisher
{
    class Program
    {
        private static readonly ILogger Logger = new Logger();
        // Build executor
        private static readonly IBuildStepExecutorFactory BuildStepExecutorFactory = new BuildStepExecutorFactory(BuildStepConfiguration.BuildStepExecutors);
        private static readonly IBuildExecutor BuildExecutor = new BuildExecutor.BuildExecutor(BuildStepExecutorFactory, Logger);
        private static readonly IBuildStepValidatorFactory BuildStepValidatorFactory = new BuildStepValidatorFactory(BuildStepConfiguration.BuildStepValidators);
        private static readonly IConfigurationValidator ConfigurationValidator = new ConfigurationValidator(BuildStepValidatorFactory);
        private static readonly IStorageAccessor StorageAccessor = new StorageAccessor();

        // Configuration
        private static readonly IConfigurationFactory ConfigurationFactory = new ConfigurationFactory(BuildStepConfiguration.BuildStepModelCreators, ConfigurationValidator);
        static void Main(string[] args)
        {
            try
            {

                Logger.LogEvent(SPublisherEvent.SPublisherStarted);
                var json = StorageAccessor.ReadAllText("spublisher.json");
                var model = ConfigurationFactory.Get(json);

                if (model.BuildSteps.Any())
                {
                    Logger.LogEvent(SPublisherEvent.BuildExecutionStarted);
                    BuildExecutor.Execute(model.BuildSteps);
                    Logger.LogEvent(SPublisherEvent.BuildExecutionCompleted);
                }

                Logger.LogEvent(SPublisherEvent.SPublisherCompleted);
            }
            catch (SPublisherException ex)
            {
                switch (ex)
                {
                    case BuildStepTypeNotFoundException buildStepTypeNotFoundException:
                        Logger.LogError(SPublisherEvent.BuildStepTypeNotFound, new BuildStepTypeNotFoundMessage(buildStepTypeNotFoundException.Type));
                        break;
                    case FileNotFoundException fileNotFoundException:
                        Logger.LogError(SPublisherEvent.FileNotFound, fileNotFoundException);
                        break;
                    case ValidationException validationException:
                        Logger.LogValidationError(validationException.ValidationInfo);
                        break;
                    default:
                        Logger.LogError(ex.SPublisherEvent);
                        break;
                }
            }
            catch (Exception)
            {
                Logger.LogError(SPublisherEvent.UnknownError);
            }
        }

        public static bool IsAdministratorMode()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
