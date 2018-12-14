using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration
{
    public interface IBuildStepValidator
    {
        ValidationErrorType[] Validate(IBuildStep step);
    }
}