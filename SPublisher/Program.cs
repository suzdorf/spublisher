using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using SPublisher.BuildExecutor;
using SPublisher.Configuration;
using SPublisher.Configuration.Exceptions;
using SPublisher.Core;

namespace SPublisher
{
    class Program
    {
        private static readonly ILogger Logger = new Logger();
        // Build executor
        private static readonly IBuildStepExecutorFactory BuildStepExecutorFactory =  new BuildStepExecutorFactory(BuildStepConfiguration.BuildStepExecutors);
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
            catch (FileNotFoundException)
            {
                Logger.LogError(SPublisherEvent.SpublisherJsonNotFound);
            }
            catch (ValidationException ex)
            {
                Logger.LogValidationError(ex.ValidationInfo);
            }
            catch (InvalidJsonException)
            {
                Logger.LogError(SPublisherEvent.InvalidJson);
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
