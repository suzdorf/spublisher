using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration
{
    public class BuildStepValidationResult : IBuildStepValidationResult
    {
        public BuildStepValidationResult(IValidationError[] errors, IBuildStep buildStep)
        {
            Errors = errors;
            BuildStep = buildStep;
        }

        public IValidationError[] Errors { get; }
        public IBuildStep BuildStep { get; }
    }
}