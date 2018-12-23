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

        public bool DataBaseExists(string dbName)
        {
            using (var connection = new SqlConnection(_connectionAccessor.ConnectionString))
            {
                using (var command = new SqlCommand($"SELECT db_id('{dbName}')", connection))
                {
                    connection.Open();
                    return command.ExecuteScalar() != DBNull.Value;
                }
            }
        }

        public void CreateDataBase(IDatabaseCreate databaseCreate)
        {
            using (var connection = new SqlConnection(_connectionAccessor.ConnectionString))
            {
                using (var command = new SqlCommand($"CREATE DATABASE {databaseCreate.DbName}", connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}