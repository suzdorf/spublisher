using System.Data.SqlClient;

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

        public static class MySql
        {
            public static string FindDatabaseScript(string databaseName)
            {
                return $"SHOW DATABASES LIKE '{databaseName}';";
            }
        }

        public static class MsSql
        {
            public static string FindDatabaseScript(string databaseName)
            {
                return $"SELECT db_id('{databaseName}');";
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
        }
    }
}