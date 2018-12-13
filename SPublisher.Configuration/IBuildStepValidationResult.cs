using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration
{
    public interface IBuildStepValidationResult
    {
        ValidationErrorType[] Errors { get; }

        IBuildStep BuildStep { get; }
    }
}