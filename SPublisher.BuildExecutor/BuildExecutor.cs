using System.Linq;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.Enums;

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
                var result = executor.Execute(buildStep);

                if (result == ExecutionResult.Success)
                {
                    _logger.LogEvent(SPublisherEvent.BuildStepExecutionCompleted, buildStep);
                }
                else
                {
                    _logger.LogError(SPublisherEvent.BuildStepExecutionCompletedWithError, buildStep);

                    buildSteps.SkipWhile(x => x != buildStep).Skip(1).ToList()
                        .ForEach(x => _logger.LogEvent(SPublisherEvent.BuildStepWasSkipped, x));

                    return;
                }
            }
        }
    }
}
