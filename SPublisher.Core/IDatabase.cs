namespace SPublisher.Core
{
    public interface IDatabase : ILogMessage
    {
        string DatabaseName { get; }
        IScripts[] Scripts { get; }
        bool HashingEnabled { get; }
    }
}