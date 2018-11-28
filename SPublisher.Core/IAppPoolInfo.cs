namespace SPublisher.Core
{
    public interface IAppPoolInfo
    {
        string AppPoolName { get; }
        string ManagedRuntimeVersion { get; }
    }
}