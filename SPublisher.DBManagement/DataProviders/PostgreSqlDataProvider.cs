using Npgsql;
using SPublisher.Core;

namespace SPublisher.DBManagement.DataProviders
{
    public class PostgreSqlDataProvider : ISqlServerDataProvider
    {
        private readonly IConnectionAccessor _connectionAccessor;

        public PostgreSqlDataProvider(IConnectionAccessor connectionAccessor)
        {
            _connectionAccessor = connectionAccessor;
        }

        public bool DataBaseExists(string databaseName)
        {
            using (var connection = new NpgsqlConnection(_connectionAccessor.ConnectionString))
            {
                using (var command = new NpgsqlCommand(SqlHelpers.PostgreSql.FindDatabaseScript(databaseName), connection))
                {
                    connection.Open();
                    return command.ExecuteScalar() != null;
                }
            }
        }

        public void CreateDataBase(IDatabase database)
        {
            ExecuteNonQuery(_connectionAccessor.ConnectionString, SqlHelpers.CreateDatabaseScript(database.DatabaseName));
        }

        public void ExecuteScript(string script, string databaseName)
        {
            var connectionString = 
                string.IsNullOrEmpty(databaseName)
                ? _connectionAccessor.ConnectionString
                : SqlHelpers.PostgreSql.SwitchConnectionStringToAnotherDatabase(
                    _connectionAccessor.ConnectionString, databaseName);

            ExecuteNonQuery(connectionString, script);
        }

        private void ExecuteNonQuery(string connectionString, string script)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand(script, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}