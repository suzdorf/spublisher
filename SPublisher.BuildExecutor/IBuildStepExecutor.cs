using SPublisher.Core.BuildSteps;

namespace SPublisher.BuildExecutor
{
    public interface IBuildStepExecutor
    {
        ExecutionResult Execute(IBuildStep buildStep);
    }
}