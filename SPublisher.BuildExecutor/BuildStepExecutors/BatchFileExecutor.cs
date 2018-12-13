using SPublisher.Core.BuildSteps;

namespace SPublisher.BuildExecutor.BuildStepExecutors
{
    public class BatchFileExecutor : IBuildStepExecutor
    {
        public ExecutionResult Execute(IBuildStep buildStep)
        {
            return ExecutionResult.Success;
        }
    }
}