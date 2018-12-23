using SPublisher.Core;

namespace SPublisher.DBManagement
{
    public class DbConnection : IConnectionAccessor, IConnectionSetter
    {
        public string ConnectionString { get; private set; }
        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}