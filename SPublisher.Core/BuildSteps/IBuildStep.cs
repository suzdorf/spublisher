namespace SPublisher.Core.BuildSteps
{
    public interface IBuildStep : ILogMessage
    {
        string Name { get; }
        string Type { get; }
    }
}
