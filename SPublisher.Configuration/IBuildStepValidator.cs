using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration
{
    public interface IBuildStepValidator
    {
        IBuildStepValidationResult Validate(IBuildStep step);
    }
}