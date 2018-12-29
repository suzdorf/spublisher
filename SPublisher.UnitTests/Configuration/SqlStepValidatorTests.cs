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
        private const string Path = "Path";
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
                result.Should().NotContain(x => x.Type == ValidationErrorType.SqlStepConnectionStringIsRequired);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.SqlStepConnectionStringIsRequired);
            }
        }

        [Theory]
        [InlineData(Path, true)]
        [InlineData("", false)]
        public void ShouldValidatePath(string path, bool isValid)
        {
            var scripts = new Mock<IScripts>();
            var database = new Mock<IDatabase>();
            scripts.SetupGet(x => x.Path).Returns(path);
            database.Setup(x => x.Scripts).Returns(new[]
            {
                scripts.Object
            });
            _buildStepMock.As<ISqlStep>().Setup(x => x.Databases).Returns(new []
            {
                database.Object
            });
            var result = _validator.Validate(_buildStepMock.Object);

            if (isValid)
            {
                result.Should().NotContain(x => x.Type == ValidationErrorType.SqlStepPathValueIsRequired);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.SqlStepPathValueIsRequired);
            }
        }
    }
}