namespace SPublisher.Core.BuildSteps
{
    public interface IBuildStep
    {
        string Name { get; }
        string Type { get; }
    }
}
