using SPublisher.Core;
using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration
{
    public interface IBuildStepValidationResult
    {
        IValidationError[] Errors { get; }

        IBuildStep BuildStep { get; }
    }
}