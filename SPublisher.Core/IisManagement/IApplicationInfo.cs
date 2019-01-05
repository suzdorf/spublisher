namespace SPublisher.Core.IisManagement
{
    public interface IApplicationInfo : ILogMessage
    {
        string Name { get; }
        string Path { get; }
        string AppPoolName { get; }
    }
}