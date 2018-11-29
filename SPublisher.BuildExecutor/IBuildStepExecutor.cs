using SPublisher.Core.BuildSteps;

namespace SPublisher.BuildExecutor
{
    public interface IBuildStepExecutor
    {
        void Execute(IBuildStep buildStep);
    }
}