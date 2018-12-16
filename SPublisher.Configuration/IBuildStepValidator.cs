using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration
{
    public interface IBuildStepValidator
    {
        IValidationError[] Validate(IBuildStep step);
    }
}