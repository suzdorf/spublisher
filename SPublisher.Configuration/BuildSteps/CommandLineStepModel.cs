using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration.BuildSteps
{
    public class CommandLineStepModel : BuildStepModel, ICommandLineStep
    {
        public bool RunAsAdministrator { get; set; }
        public string[] Commands { get; set; }
    }
}
