using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration.BuildSteps
{
    public class BuildStepModel : IBuildStep
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
