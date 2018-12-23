namespace SPublisher.Core.BuildSteps
{
    public interface ISqlStep : IBuildStep
    {
        string ConnectionString { get; }
        IDatabaseCreate[] DatabaseCreate { get; }
    }
}