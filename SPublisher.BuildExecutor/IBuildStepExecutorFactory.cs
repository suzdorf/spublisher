using SPublisher.Core.BuildSteps;

namespace SPublisher.BuildExecutor
{
    public interface IBuildStepExecutorFactory
    {
        IBuildStepExecutor Get(IBuildStep buildStep);
    }
}
