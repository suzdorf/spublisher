using SPublisher.Core.Log;

namespace SPublisher.DBManagement
{
    public interface ISqlScriptInfo : ILogMessage
    {
        string Path { get; }
    }
}