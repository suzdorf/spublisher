using System;
using System.Data.SqlClient;
using SPublisher.Core;

namespace SPublisher.DBManagement
{
    public static class SqlHelpers
    {
        public const string MasterDatabaseName = "master";
        public const string SqlFileExtension = ".sql";

        public static string UseDatabaseScript(string script, string databaseName)
        {
            return $"USE {databaseName}; {script}";
        }

        public static string CreateDatabaseScript(string databaseName)
        {
            return $"CREATE DATABASE {databaseName};";
        }

        public static string GetHashInfoScript()
        {
            return "SELECT Hash FROM SpublisherHashInfo;";
        }

        public static string SaveHashInfoScript(IFile hashInfo)
        {
            return
                $"INSERT INTO SpublisherHashInfo (Path, Hash, DateExecuted)\r\nVALUES (\'{hashInfo.Path}\', \'{hashInfo.Hash}\', \'{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}\');";
        }

        public static class MySql
        {
            public static string FindDatabaseScript(string databaseName)
            {
                return $"SHOW DATABASES LIKE '{databaseName}';";
            }

            public static string CreateHashInfoTableScript()
            {
                return "CREATE TABLE IF NOT EXISTS SpublisherHashInfo (\r\n   Path varchar(1000),\r\n    Hash varchar(1000) not null,\r\n    DateExecuted datetime);";
            }
        }

        public static class MsSql
        {
            public static string FindDatabaseScript(string databaseName)
            {
                return $"SELECT db_id('{databaseName}');";
            }

            public static string CreateHashInfoTableScript()
            {
                return
                    "if not exists (select * from sysobjects where name='SpublisherHashInfo' and xtype='U')" +
                    "CREATE TABLE SpublisherHashInfo (\r\n   Path varchar(max),\r\n    Hash varchar(max) not null,\r\n    DateExecuted datetime);";
            }
        }

        public static class PostgreSql
        {
            public static string FindDatabaseScript(string databaseName)
            {
                return $"SELECT 1 FROM pg_database WHERE datname='{databaseName}';";
            }

            public static string SwitchConnectionStringToAnotherDatabase(string connectionString, string databaseName)
            {
                return System.Text.RegularExpressions.Regex.Replace(connectionString, "Database=([^=;]*)", $"Database={databaseName}");
            }

            public static string CreateHashInfoTableScript()
            {
                return
                    "CREATE TABLE SpublisherHashInfo (\r\n   Path varchar,\r\n    Hash varchar not null,\r\n    DateExecuted timestamp);";
            }
        }
    }
}