using SPublisher.Core.DbManagement;
using SPublisher.Core.Enums;

namespace SPublisher.DBManagement
{
    public class DbConnection : IConnectionAccessor, IConnectionSetter
    {
        public string ConnectionString { get; private set; }
        public void Set(ISqlConnectionSettings connectionSettings)
        {
            ConnectionString = connectionSettings.ConnectionString;
            ServerType = connectionSettings.ServerType;
        }

        public SqlServerType ServerType { get; private set; }
    }
}