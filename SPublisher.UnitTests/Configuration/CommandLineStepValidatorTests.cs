using FluentAssertions;
using Moq;
using SPublisher.Configuration;
using SPublisher.Configuration.BuildStepValidators;
using SPublisher.Core.BuildSteps;
using Xunit;

namespace SPublisher.UnitTests.Configuration
{
    public class CommandLineStepValidatorTests
    {
        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void ShouldValidateRunAsAdministrator(bool isAdministratorMode, bool runAsAdministrator, bool isValid)
        {
            var validator = new CommandLineStepValidator(isAdministratorMode);
            var buildStepMock = new Mock<IBuildStep>();
            buildStepMock.As<ICommandLineStep>().SetupGet(x => x.RunAsAdministrator).Returns(runAsAdministrator);
            var result = validator.Validate(buildStepMock.Object);
            if (isValid)
            {
                result.Should().NotContain(ValidationErrorType.ShouldRunAsAdministrator);
            }
            else
            {
                result.Should().Contain(ValidationErrorType.ShouldRunAsAdministrator);
            }
        }
    }
}