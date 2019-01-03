using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SPublisher.Core;
using SPublisher.DBManagement.Models;

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

        public void CreateHashInfoTableIfNotExists(string databaseName)
        {
            ExecuteNonQuery(SqlHelpers.UseDatabaseScript(SqlHelpers.MySql.CreateHashInfoTableScript(), databaseName));
        }

        public IScriptHashInfo[] GetHashInfoList(string databaseName)
        {
            var result = new List<IScriptHashInfo>();
            using (var connection = new MySqlConnection(_connectionAccessor.ConnectionString))
            {
                using (var command = new MySqlCommand(SqlHelpers.UseDatabaseScript(SqlHelpers.GetHashInfoScript(), databaseName), connection))
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new ScriptHashModel { Hash = reader["Hash"].ToString() });
                        }
                    }
                }
            }

            return result.ToArray();
        }

        public void SaveHashInfo(string databaseName, IFile hashInfo)
        {
            ExecuteNonQuery(SqlHelpers.UseDatabaseScript(SqlHelpers.SaveHashInfoScript(hashInfo), databaseName));
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