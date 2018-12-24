namespace SPublisher.Core.BuildSteps
{
    public interface ISqlStep : IBuildStep
    {
        string ConnectionString { get; }
        IDatabase[] Databases { get; }
    }
}