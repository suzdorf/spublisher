using System;
using System.Data.SqlClient;
using SPublisher.Core;

namespace SPublisher.DBManagement
{
    public class SqlServerDataProvider : ISqlServerDataProvider
    {
        private readonly IConnectionAccessor _connectionAccessor;

        public SqlServerDataProvider(IConnectionAccessor connectionAccessor)
        {
            _connectionAccessor = connectionAccessor;
        }

        public bool DataBaseExists(string databaseName)
        {
            using (var connection = new SqlConnection(_connectionAccessor.ConnectionString))
            {
                using (var command = new SqlCommand(SqlHelpers.FindDatabaseScript(databaseName), connection))
                {
                    connection.Open();
                    return command.ExecuteScalar() != DBNull.Value;
                }
            }
        }

        public void CreateDataBase(IDatabase database)
        {
            ExecuteNonQuery(SqlHelpers.CreateDatabaseScript(database.DatabaseName));
        }

        public void ExecuteScript(string script, string databaseName)
        {
            ExecuteNonQuery(SqlHelpers.UseDatabaseScript(script, databaseName));
        }

        private void ExecuteNonQuery(string script)
        {
            using (var connection = new SqlConnection(_connectionAccessor.ConnectionString))
            {
                using (var command = new SqlCommand(script, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}