using FluentAssertions;
using Moq;
using SPublisher.Configuration;
using SPublisher.Configuration.BuildStepValidators;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using Xunit;

namespace SPublisher.UnitTests.Configuration
{
    public class SqlStepValidatorTests
    {
        private const string ConnectionString = "ConnectionString";
        private const string DataBaseName = "DataBaseName";
        private readonly Mock<IBuildStep> _buildStepMock = new Mock<IBuildStep>();
        private readonly IBuildStepValidator _validator = new SqlStepValidator(); 

        [Theory]
        [InlineData(ConnectionString, true)]
        [InlineData("", false)]
        public void ShouldValidateConnectionString(string connectionString, bool isValid)
        {
            _buildStepMock.As<ISqlStep>().Setup(x => x.ConnectionString).Returns(connectionString);

            var result = _validator.Validate(_buildStepMock.Object);

            if (isValid)
            {
                result.Should().NotContain(x => x.Type == ValidationErrorType.ConnectionStringIsRequired);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.ConnectionStringIsRequired);
            }
        }

        [Fact]
        public void ShouldValidateUniqueDbNames()
        {
            var dbCreateMock = new Mock<IDatabaseCreate>();
            dbCreateMock.SetupGet(x => x.DbName).Returns(DataBaseName);
            _buildStepMock.As<ISqlStep>().Setup(x => x.DatabaseCreate).Returns(new[]
            {
                dbCreateMock.Object,
                dbCreateMock.Object
            });

            var result = _validator.Validate(_buildStepMock.Object);
            result.Should().Contain(x => x.Type == ValidationErrorType.DbNamesShouldBeUnique);
        }

        [Theory]
        [InlineData(DataBaseName, true)]
        [InlineData("", false)]
        public void ShouldValidateDbName(string dbName, bool isValid)
        {
            var dbCreateMock = new Mock<IDatabaseCreate>();
            dbCreateMock.SetupGet(x => x.DbName).Returns(dbName);
            _buildStepMock.As<ISqlStep>().Setup(x => x.DatabaseCreate).Returns(new[]
            {
                dbCreateMock.Object
            });


            var result = _validator.Validate(_buildStepMock.Object);

            if (isValid)
            {
                result.Should().NotContain(x => x.Type == ValidationErrorType.DbNameIsRequired);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.DbNameIsRequired);
            }
        }
    }
}