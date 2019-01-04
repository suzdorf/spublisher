using System;
using System.Collections.Generic;
using Npgsql;
using SPublisher.Core;
using SPublisher.DBManagement.Models;

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

        public void RestoreDatabase(IDatabase database)
        {
            throw new NotImplementedException();
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

        public void CreateHashInfoTableIfNotExists(string databaseName)
        {
            try
            {
                ExecuteNonQuery(SqlHelpers.PostgreSql.SwitchConnectionStringToAnotherDatabase(
                    _connectionAccessor.ConnectionString, databaseName), SqlHelpers.PostgreSql.CreateHashInfoTableScript());
            }
            catch (NpgsqlException){}
        }

        public IScriptHashInfo[] GetHashInfoList(string databaseName)
        {
            var result = new List<IScriptHashInfo>();
            using (var connection = new NpgsqlConnection(SqlHelpers.PostgreSql.SwitchConnectionStringToAnotherDatabase(
                _connectionAccessor.ConnectionString, databaseName)))
            {
                using (var command = new NpgsqlCommand(SqlHelpers.GetHashInfoScript(), connection))
                {
                    connection.Open();
                    using (NpgsqlDataReader reader = command.ExecuteReader())
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
            ExecuteNonQuery(SqlHelpers.PostgreSql.SwitchConnectionStringToAnotherDatabase(
                _connectionAccessor.ConnectionString, databaseName), SqlHelpers.SaveHashInfoScript(hashInfo));
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