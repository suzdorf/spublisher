namespace SPublisher.Core.BuildSteps
{
    public interface IIisManagementStep : IBuildStep
    {
        IApplication[] Applications { get; }
    }
}