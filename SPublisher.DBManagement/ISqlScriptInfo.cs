using SPublisher.Core;

namespace SPublisher.DBManagement
{
    public interface ISqlScriptInfo : ILogMessage
    {
        string Path { get; }
    }
}