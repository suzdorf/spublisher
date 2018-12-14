using System;
using System.Linq;
using FluentAssertions;
using Moq;
using SPublisher.Configuration;
using SPublisher.Configuration.Exceptions;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using Xunit;

namespace SPublisher.UnitTests.Configuration
{
    public class ConfigurationValidatorTests
    {
        private readonly Mock<IBuildStepValidatorFactory> _validatorFactoryMock;
        private readonly IConfigurationValidator _configurationValidator;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IBuildStep> _firstStepMock;
        private readonly Mock<IBuildStep> _secondStepMock;
        private readonly Mock<IBuildStepValidator> _firstStepValidatorMock;
        private readonly Mock<IBuildStepValidator> _secondStepValidatorMock;
        private readonly Mock<IBuildStepValidationResult> _firstStepResult;
        private readonly Mock<IBuildStepValidationResult> _secondStepResult;
        private readonly IBuildStep[] _buildSteps;

        public ConfigurationValidatorTests()
        {
            _firstStepMock = new Mock<IBuildStep>();
            _secondStepMock = new Mock<IBuildStep>();
            _firstStepValidatorMock = new Mock<IBuildStepValidator>();
            _secondStepValidatorMock =  new Mock<IBuildStepValidator>();
            _firstStepResult = new Mock<IBuildStepValidationResult>();
            _secondStepResult =  new Mock<IBuildStepValidationResult>();
            _firstStepValidatorMock.Setup(x => x.Validate(_firstStepMock.Object)).Returns(_firstStepResult.Object);
            _secondStepValidatorMock.Setup(x => x.Validate(_secondStepMock.Object)).Returns(_secondStepResult.Object);
            _buildSteps = new[]
            {
                _firstStepMock.Object,
                _secondStepMock.Object
            };
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.SetupGet(x => x.BuildSteps).Returns(_buildSteps);
            _validatorFactoryMock = new Mock<IBuildStepValidatorFactory>();
            _validatorFactoryMock.Setup(x => x.Get(_firstStepMock.Object)).Returns(_firstStepValidatorMock.Object);
            _validatorFactoryMock.Setup(x => x.Get(_secondStepMock.Object)).Returns(_secondStepValidatorMock.Object);
            _configurationValidator = new ConfigurationValidator(_validatorFactoryMock.Object);
        }

        [Fact]
        public void ShouldNotThrowValidationExceptionIfNoErrors()
        {
            _firstStepResult.SetupGet(x => x.Errors).Returns(new ValidationErrorType[0]);
            _secondStepResult.SetupGet(x => x.Errors).Returns(new ValidationErrorType[0]);
            Action action = () => { _configurationValidator.Validate(_configurationMock.Object); };
            action.Should().NotThrow<ValidationException>();
        }

        [Fact]
        public void ShouldThrowValidationExceptionIfThereAreAnyErrors()
        {
            _firstStepResult.SetupGet(x => x.Errors).Returns(new ValidationErrorType[0]);
            _secondStepResult.SetupGet(x => x.Errors).Returns(new[] { ValidationErrorType.ShouldRunAsAdministrator });
            Action action = () => { _configurationValidator.Validate(_configurationMock.Object); };
            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void ShouldNotCallValidatorIfItIsNull()
        {
            _validatorFactoryMock.Setup(x => x.Get(_firstStepMock.Object)).Returns((IBuildStepValidator) null);
            _secondStepResult.SetupGet(x => x.Errors).Returns(new[] { ValidationErrorType.ShouldRunAsAdministrator });

            Action action = () => { _configurationValidator.Validate(_configurationMock.Object); };
            action.Should().Throw<ValidationException>().Which.ValidationResults.Count.Should().Be(1);
        }

        [Fact]
        public void ShouldCollectAllErrorsWithinException()
        {
            _firstStepResult.SetupGet(x => x.Errors).Returns(new[] { ValidationErrorType.ShouldRunAsAdministrator , ValidationErrorType.ShouldRunAsAdministrator });
            _secondStepResult.SetupGet(x => x.Errors).Returns(new[] { ValidationErrorType.ShouldRunAsAdministrator });

            Action action = () => { _configurationValidator.Validate(_configurationMock.Object); };
            var validationResults = action.Should().Throw<ValidationException>().Which.ValidationResults;
            validationResults.Count.Should().Be(2);
            validationResults.First().Errors.Length.Should().Be(2);
            validationResults.Last().Errors.Length.Should().Be(1);
        }
    }
}