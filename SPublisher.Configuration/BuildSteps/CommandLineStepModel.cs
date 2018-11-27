using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration.BuildSteps
{
    public class CommandLineStepModel : BuildStepModel, ICommandLineStep
    {
        public string[] Commands { get; set; }
    }
}
