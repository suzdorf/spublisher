namespace SPublisher.Core
{
    public interface IAppPoolInfo : ILogMessage
    {
        string AppPoolName { get; }
        string ManagedRuntimeVersion { get; }
    }
}