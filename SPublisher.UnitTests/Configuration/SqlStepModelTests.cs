using FluentAssertions;
using SPublisher.Configuration.BuildSteps;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using Xunit;

namespace SPublisher.UnitTests.Configuration
{
    public class SqlStepModelTests
    {
        [Theory]
        [InlineData(null, SqlServerType.MsSql)]
        [InlineData("", SqlServerType.MsSql)]
        [InlineData(Constants.SqlServerType.MsSql, SqlServerType.MsSql)]
        [InlineData(Constants.SqlServerType.MySql, SqlServerType.MySql)]
        [InlineData(Constants.SqlServerType.PostgreSql, SqlServerType.PostgreSql)]
        [InlineData("randomstring", SqlServerType.Invalid)]
        public void SqlServerTypeParsing(string type, SqlServerType result)
        {
            var model = new SqlStepModel
            {
                ServerType = type
            };
            ((ISqlStep)model).ServerType.Should().BeEquivalentTo(result);
        }
    }
}