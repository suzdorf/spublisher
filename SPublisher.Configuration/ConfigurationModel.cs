using SPublisher.Configuration.BuildSteps;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration
{
    public class ConfigurationModel : IConfiguration
    {
        public bool HashingEnabled { get; set; } = true;
        public BuildStepModel[] BuildSteps { get; set; }

        IBuildStep[] IConfiguration.BuildSteps => BuildSteps;
    }
}
