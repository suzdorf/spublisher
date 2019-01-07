using SPublisher.Core.Log;

namespace SPublisher.Core.DbManagement
{
    public interface IDatabase : ILogMessage
    {
        string DatabaseName { get; }
        string BackupPath { get; }
        IScripts[] Scripts { get; }
        bool HashingEnabled { get; }
        bool RestoreAvailable { get; }
    }
}