using System.Linq;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;
using SPublisher.Core.Log;

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

            if (step.Sites.Any())
            {
                _logger.LogEvent(SPublisherEvent.IisManagementStarted);
                _siteCreator.Create(step.Sites);
                _logger.LogEvent(SPublisherEvent.IisManagementCompleted);
            }

            return ExecutionResult.Success;
        }
    }
}