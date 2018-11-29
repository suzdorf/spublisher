using SPublisher.Core.BuildSteps;

namespace SPublisher.Core
{
    public interface IBuildExecutor
    {
        void Execute(IBuildStep[] buildSteps);
    }
}
