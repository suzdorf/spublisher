using SPublisher.Core.Enums;

namespace SPublisher.Core.DbManagement
{
    public interface ISqlConnectionSettings
    {
        string ConnectionString { get; }
        SqlServerType ServerType { get; }
    }
}