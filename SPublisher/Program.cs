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
        private static readonly IStorageAccessor StorageAccessor = new StorageAccessor();
        private static readonly IStorageLogger StorageLogger = new StorageLogger(StorageAccessor, LocalFolderPath);
        private static readonly ILogger Logger = new Logger(StorageLogger);
        // Build executor
        private static readonly IBuildStepExecutorFactory BuildStepExecutorFactory = new BuildStepExecutorFactory(BuildStepConfiguration.BuildStepExecutors);
        private static readonly IBuildExecutor BuildExecutor = new BuildExecutor.BuildExecutor(BuildStepExecutorFactory, Logger);
        private static readonly IBuildStepValidatorFactory BuildStepValidatorFactory = new BuildStepValidatorFactory(BuildStepConfiguration.BuildStepValidators);
        private static readonly IConfigurationValidator ConfigurationValidator = new ConfigurationValidator(BuildStepValidatorFactory);

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
                    case ValidationException validationException:
                        Logger.LogValidationError(validationException.ValidationInfo);
                        break;
                    default:
                        if (ex is ILogMessage message)
                        {
                            Logger.LogError(ex.SPublisherEvent, message);
                        }
                        else
                        {
                            Logger.LogError(ex.SPublisherEvent);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public static bool IsAdministratorMode
        {
            get
            {
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static string LocalFolderPath => $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\spublisher";
    }
}
