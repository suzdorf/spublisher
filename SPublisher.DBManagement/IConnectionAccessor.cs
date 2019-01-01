using SPublisher.Core;

namespace SPublisher.DBManagement
{
    public interface IConnectionAccessor
    {
        string ConnectionString { get; }
        SqlServerType ServerType { get; }
    }
}