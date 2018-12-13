using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration
{
    public class BuildStepValidationResult : IBuildStepValidationResult
    {
        public BuildStepValidationResult(ValidationErrorType[] errors, IBuildStep buildStep)
        {
            Errors = errors;
            BuildStep = buildStep;
        }

        public ValidationErrorType[] Errors { get; }
        public IBuildStep BuildStep { get; }
    }
}