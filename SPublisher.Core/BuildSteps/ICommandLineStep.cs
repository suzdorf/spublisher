
namespace SPublisher.Core.BuildSteps
{
    public interface ICommandLineStep : IBuildStep
    {
        bool RunAsAdministrator { get; }
        string[] Commands { get; }
    }
}
