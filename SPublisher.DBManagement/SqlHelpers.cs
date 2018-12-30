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

        public static string FindDatabaseScript(string databaseName)
        {
            return $"SELECT db_id('{databaseName}');";
        }
    }
}