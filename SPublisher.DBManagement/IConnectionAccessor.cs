using SPublisher.Core;
using SPublisher.Core.Enums;

namespace SPublisher.DBManagement
{
    public interface IConnectionAccessor
    {
        string ConnectionString { get; }
        SqlServerType ServerType { get; }
    }
}