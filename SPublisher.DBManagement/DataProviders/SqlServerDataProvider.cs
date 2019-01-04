using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SPublisher.Core;
using SPublisher.DBManagement.Models;

namespace SPublisher.DBManagement.DataProviders
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
                using (var command = new SqlCommand(SqlHelpers.MsSql.FindDatabaseScript(databaseName), connection))
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

        public void RestoreDatabase(IDatabase database)
        {
            var script =
                DataBaseExists(database.DatabaseName)
                    ? SqlHelpers.RestoreExistingDatabase(database.DatabaseName, database.BackupPath)
                    : SqlHelpers.RestoreDatabase(database.DatabaseName, database.BackupPath);

            ExecuteNonQuery(script);
        }

        public void ExecuteScript(string script, string databaseName)
        {
            ExecuteNonQuery(
                string.IsNullOrEmpty(databaseName)
                    ? SqlHelpers.UseDatabaseScript(script, SqlHelpers.MasterDatabaseName)
                    : SqlHelpers.UseDatabaseScript(script, databaseName));
        }

        public void CreateHashInfoTableIfNotExists(string databaseName)
        {
            ExecuteNonQuery(SqlHelpers.UseDatabaseScript(SqlHelpers.MsSql.CreateHashInfoTableScript(), databaseName));
        }

        public IScriptHashInfo[] GetHashInfoList(string databaseName)
        {
            var result = new List<IScriptHashInfo>();
            using (var connection = new SqlConnection(_connectionAccessor.ConnectionString))
            {
                using (var command = new SqlCommand(SqlHelpers.UseDatabaseScript(SqlHelpers.GetHashInfoScript(), databaseName), connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new ScriptHashModel{Hash = reader["Hash"].ToString() });
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