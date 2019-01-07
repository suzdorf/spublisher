using SPublisher.Core.Log;

namespace SPublisher.Core.IisManagement
{
    public interface IAppPoolInfo : ILogMessage
    {
        string AppPoolName { get; }
        string ManagedRuntimeVersion { get; }
    }
}