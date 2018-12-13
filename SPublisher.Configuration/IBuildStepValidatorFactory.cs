using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration
{
    public interface IBuildStepValidatorFactory
    {
        IBuildStepValidator Get(IBuildStep step);
    }
}