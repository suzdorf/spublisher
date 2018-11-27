
namespace SPublisher.Core.BuildSteps
{
    public interface ICommandLineStep : IBuildStep
    {
        string[] Commands { get; }
    }
}
