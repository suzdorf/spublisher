using System.Linq;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;

namespace SPublisher.BuildExecutor.BuildStepExecutors
{
    public class IisManagementExecutor : IBuildStepExecutor
    {
        private readonly ISiteCreator _siteCreator;
        private readonly ILogger _logger;

        public IisManagementExecutor(ISiteCreator siteCreator, ILogger logger)
        {
            _siteCreator = siteCreator;
            _logger = logger;
        }

        public ExecutionResult Execute(IBuildStep buildStep)
        {
            var step = (IIisManagementStep) buildStep;

            if (step.Applications.Any())
            {
                _logger.LogEvent(SPublisherEvent.IisManagementStarted);
                _siteCreator.Create(step.Applications);
                _logger.LogEvent(SPublisherEvent.IisManagementCompleted);
            }

            return ExecutionResult.Success;
        }
    }
}