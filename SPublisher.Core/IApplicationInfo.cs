namespace SPublisher.Core
{
    public interface IApplicationInfo
    {
        string Name { get; }
        string Path { get; }
        string AppPoolName { get; }
    }
}