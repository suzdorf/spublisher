using SPublisher.Configuration.BuildSteps;
using SPublisher.Configuration.IISObjects;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
// ReSharper disable CoVariantArrayConversion

namespace SPublisher.Configuration
{
    class ConfigurationModel : IConfiguration
    {
        public BuildStepModel[] BuildSteps { get; set; }

        public ApplicationModel[] Applications { get; set; }

        IBuildStep[] IConfiguration.BuildSteps
        {
            get { return BuildSteps; }
        }

        IApplication[] IConfiguration.Applications
        {
            get { return Applications; }
        }
    }
}
