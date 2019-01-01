using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using SPublisher.Configuration;
using SPublisher.Configuration.Exceptions;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.Exceptions;
using Xunit;

namespace SPublisher.UnitTests
{
    public class SpublisherRunnerTests
    {
        private const string ConfigurationFileName = "ConfigurationFileName";
        private const string ConfigurationJson = "ConfigurationJson";
        private readonly string[] _args = new string[0];
        private readonly Mock<IRunOptionsFactory> _runOptionsFactoryMock =  new Mock<IRunOptionsFactory>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly Mock<IStorageAccessor> _storageAccessorMock = new Mock<IStorageAccessor>();
        private readonly Mock<IConfigurationFactory> _configurationFactoryMock = new Mock<IConfigurationFactory>();
        private readonly Mock<IBuildExecutor> _buildExecutorMock = new Mock<IBuildExecutor>();
        private readonly Mock<IRunOptions> _runOptionsMock = new Mock<IRunOptions>();
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
        private readonly SPublisherRunner _sPublisherRunner;

        public SpublisherRunnerTests()
        {
            _storageAccessorMock.Setup(x => x.ReadAllText(ConfigurationFileName)).Returns(ConfigurationJson);
            _runOptionsMock.SetupGet(x => x.ConfigurationFileName).Returns(ConfigurationFileName);
            _configurationFactoryMock.Setup(x => x.Get(ConfigurationJson)).Returns(_configurationMock.Object);
            _runOptionsFactoryMock.Setup(x => x.Get(_args)).Returns(_runOptionsMock.Object);
            _sPublisherRunner =  new SPublisherRunner(
                _runOptionsFactoryMock.Object,
                _loggerMock.Object,
                _storageAccessorMock.Object,
                _buildExecutorMock.Object,
                _configurationFactoryMock.Object);
        }

        [Fact]
        public void ShouldNotExecuteBuildStepsInCaseThereAreNo()
        {
            _configurationMock.SetupGet(x => x.BuildSteps).Returns(new IBuildStep[0]);
            _sPublisherRunner.Run(_args);

            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.BuildExecutionStarted, null), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.BuildExecutionCompleted, null), Times.Never);
            _buildExecutorMock.Verify(x => x.Execute(_configurationMock.Object.BuildSteps), Times.Never);
        }

        [Fact]
        public void ShouldExecuteBuildStepsInCaseThereAreAny()
        {
            _configurationMock.SetupGet(x => x.BuildSteps).Returns(new []
            {
                new Mock<IBuildStep>().Object 
            });
            _sPublisherRunner.Run(_args);

            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.BuildExecutionStarted, null), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.BuildExecutionCompleted, null), Times.Once);
            _buildExecutorMock.Verify(x => x.Execute(_configurationMock.Object.BuildSteps), Times.Once);
        }

        [Fact]
        public void ShouldParseCommandLineArgsReadJsonFromStorageAndParseConfigurationModel()
        {
            _configurationMock.SetupGet(x => x.BuildSteps).Returns(new IBuildStep[0]);
            _sPublisherRunner.Run(_args);

            _runOptionsFactoryMock.Verify(x=>x.Get(_args), Times.Once);
            _storageAccessorMock.Verify(x=>x.ReadAllText(ConfigurationFileName), Times.Once);
            _configurationFactoryMock.Verify(x=>x.Get(ConfigurationJson), Times.Once);
        }

        [Fact]
        public void ShouldHandleExceptionAndLog()
        {
            var exception = new Exception();
            _runOptionsFactoryMock.Setup(x => x.Get(_args)).Throws(exception);

            Action action = () => { _sPublisherRunner.Run(_args); };
            action.Should().NotThrow();
            _loggerMock.Verify(x => x.LogError(exception), Times.Once);
            _loggerMock.Verify(x => x.LogError(SPublisherEvent.UnknownError, null), Times.Once);
        }

        [Fact]
        public void ShouldHandleSPublisherExceptionAndLog()
        {
            var exception = new SpublisherExceptionStab();
            _runOptionsFactoryMock.Setup(x => x.Get(_args)).Throws(exception);

            Action action = () => { _sPublisherRunner.Run(_args); };
            action.Should().NotThrow();
            _loggerMock.Verify(x => x.LogError(exception.SPublisherEvent, null), Times.Once);
        }

        [Fact]
        public void ShouldHandleSPublisherExceptionLogMessageAndLog()
        {
            var exception = new SpublisherExceptionLogMessageStab();
            _runOptionsFactoryMock.Setup(x => x.Get(_args)).Throws(exception);

            Action action = () => { _sPublisherRunner.Run(_args); };
            action.Should().NotThrow();
            _loggerMock.Verify(x => x.LogError(exception.SPublisherEvent, exception), Times.Once);
        }

        [Fact]
        public void ShouldHandleValidationExceptionAndLog()
        {
            var exception = new ValidationException(new List<IBuildStepValidationResult>());
            _configurationFactoryMock.Setup(x => x.Get(ConfigurationJson)).Throws(exception);

            Action action = () => { _sPublisherRunner.Run(_args); };
            action.Should().NotThrow();
            _loggerMock.Verify(x => x.LogValidationError(It.IsAny<IValidationInfo>()), Times.Once);
        }

        private class SpublisherExceptionStab : SPublisherException
        {
            public override SPublisherEvent SPublisherEvent => SPublisherEvent.UnknownError;
        }

        private class SpublisherExceptionLogMessageStab : SPublisherException, ILogMessage
        {
            public override SPublisherEvent SPublisherEvent => SPublisherEvent.UnknownError;
        }
    }
}