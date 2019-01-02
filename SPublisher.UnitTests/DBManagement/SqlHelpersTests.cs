using FluentAssertions;
using SPublisher.DBManagement;
using Xunit;

namespace SPublisher.UnitTests.DBManagement
{
    public class SqlHelpersTests
    {
        private const string ConnectionString1 =
            "Driver={PostgreSQL};Server=IP address;Port=5432;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";
        private const string Output1 =
            "Driver={PostgreSQL};Server=IP address;Port=5432;Database=DatabaseName;Uid=myUsername;Pwd=myPassword;";
        private const string ConnectionString2 =
            "Driver={PostgreSQL};Server=IP address;Port=5432;Uid=myUsername;Pwd=myPassword;Database=myDataBase";
        private const string Output2 =
            "Driver={PostgreSQL};Server=IP address;Port=5432;Uid=myUsername;Pwd=myPassword;Database=DatabaseName";
        private const string DatabaseName = "DatabaseName";

        [Theory]
        [InlineData(ConnectionString1, DatabaseName, Output1)]
        [InlineData(ConnectionString2, DatabaseName, Output2)]
        public void SwitchConnectionStringToAnotherDatabaseTest(
            string connectionString,
            string databaseName,
            string output)
        {
            SqlHelpers.PostgreSql.SwitchConnectionStringToAnotherDatabase(connectionString, databaseName).Should()
                .BeEquivalentTo(output);
        }
    }
}