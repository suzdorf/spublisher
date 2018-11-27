namespace SPublisher.Core
{
    public interface IApplication
    {
        string Name { get; }
        string AppPoolName { get; }
        string ManagedRuntimeVersion { get; }
        string Path { get; }
        IApplication[] Applications { get; }
    }
}
