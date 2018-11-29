using SPublisher.Configuration.BuildSteps;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration
{
    class ConfigurationModel : IConfiguration
    {
        public BuildStepModel[] BuildSteps { get; set; }

        IBuildStep[] IConfiguration.BuildSteps
        {
            get { return BuildSteps; }
        }
    }
}
