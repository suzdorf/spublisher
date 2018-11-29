using SPublisher.Core;
using SPublisher.Core.BuildSteps;

namespace SPublisher.BuildExecutor
{
    public class BuildExecutor : IBuildExecutor
    {
        private readonly IBuildStepExecutorFactory _buildStepExecutorFactory;
        private readonly ILogger _logger;

        public BuildExecutor(IBuildStepExecutorFactory buildStepExecutorFactory, ILogger logger)
        {
            _buildStepExecutorFactory = buildStepExecutorFactory;
            _logger = logger;
        }

        public void Execute(IBuildStep[] buildSteps)
        {
            foreach (var buildStep in buildSteps)
            {
                _logger.LogEvent(SPublisherEvent.BuildStepExecutionStarted, buildStep);
                var executor = _buildStepExecutorFactory.Get(buildStep);
                executor.Execute(buildStep);
                _logger.LogEvent(SPublisherEvent.BuildStepExecutionCompleted, buildStep);
            }
        }
    }
}
