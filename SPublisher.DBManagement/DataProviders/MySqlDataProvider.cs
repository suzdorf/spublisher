using MySql.Data.MySqlClient;
using SPublisher.Core;

namespace SPublisher.DBManagement.DataProviders
{
    public class MySqlDataProvider : ISqlServerDataProvider
    {
        private readonly IConnectionAccessor _connectionAccessor;

        public MySqlDataProvider(IConnectionAccessor connectionAccessor)
        {
            _connectionAccessor = connectionAccessor;
        }

        public bool DataBaseExists(string databaseName)
        {
            using (var connection =  new MySqlConnection(_connectionAccessor.ConnectionString))
            {
                using (var command = new MySqlCommand(SqlHelpers.MySql.FindDatabaseScript(databaseName), connection))
                {
                    connection.Open();
                    return command.ExecuteScalar() != null;
                }
            }
        }

        public void CreateDataBase(IDatabase database)
        {
            ExecuteNonQuery(SqlHelpers.CreateDatabaseScript(database.DatabaseName));
        }

        public void ExecuteScript(string script, string databaseName)
        {
            ExecuteNonQuery(
                string.IsNullOrEmpty(databaseName) ?
                    script :
                    SqlHelpers.UseDatabaseScript(script, databaseName));
        }

        private void ExecuteNonQuery(string script)
        {
            using (var connection = new MySqlConnection(_connectionAccessor.ConnectionString))
            {
                using (var command = new MySqlCommand(script, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}