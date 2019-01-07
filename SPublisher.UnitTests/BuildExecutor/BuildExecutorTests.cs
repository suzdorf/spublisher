using Moq;
using SPublisher.BuildExecutor;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.Enums;
using SPublisher.Core.Log;
using Xunit;

namespace SPublisher.UnitTests.BuildExecutor
{
    public class BuildExecutorTests
    {
        private readonly Mock<IBuildStepExecutorFactory> _buildStepExecutorFactoryMock = new Mock<IBuildStepExecutorFactory>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly Mock<IBuildStepExecutor> _buildStepExecutorMock = new Mock<IBuildStepExecutor>();
        private readonly Mock<IBuildStep> _firstStepMock =  new Mock<IBuildStep>();
        private readonly Mock<IBuildStep> _secondStepMock = new Mock<IBuildStep>();
        private readonly IBuildStep[] _buildSteps;
        private readonly IBuildExecutor _buildExecutor;

        public BuildExecutorTests()
        {
            _buildSteps = new[] {_firstStepMock.Object, _secondStepMock.Object};
            _buildStepExecutorFactoryMock.Setup(x => x.Get(It.IsAny<IBuildStep>()))
                .Returns(_buildStepExecutorMock.Object);

            _buildExecutor = new SPublisher.BuildExecutor.BuildExecutor(_buildStepExecutorFactoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ShouldExecuteEachBuildStepOneByOneLoggingBeggingAndTheEndOfBuildStepExecution()
        {
            _buildExecutor.Execute(_buildSteps);

            _buildStepExecutorMock.Verify(x=>x.Execute(_firstStepMock.Object), Times.Once);
            _buildStepExecutorMock.Verify(x => x.Execute(_secondStepMock.Object), Times.Once);
            _loggerMock.Verify(x=>x.LogEvent(SPublisherEvent.BuildStepExecutionStarted, _firstStepMock.Object), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.BuildStepExecutionStarted, _secondStepMock.Object), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.BuildStepExecutionCompleted, _firstStepMock.Object), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.BuildStepExecutionCompleted, _secondStepMock.Object), Times.Once);
        }

        [Fact]
        public void ShouldSkipOtherBuildStepsIfBuildStepFails()
        {
            _buildStepExecutorMock.Setup(x => x.Execute(_firstStepMock.Object)).Returns(ExecutionResult.Error);

            _buildExecutor.Execute(_buildSteps);

            _buildStepExecutorMock.Verify(x => x.Execute(_firstStepMock.Object), Times.Once);
            _buildStepExecutorMock.Verify(x => x.Execute(_secondStepMock.Object), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.BuildStepExecutionStarted, _firstStepMock.Object), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.BuildStepExecutionStarted, _secondStepMock.Object), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.BuildStepExecutionCompleted, _firstStepMock.Object), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.BuildStepExecutionCompleted, _secondStepMock.Object), Times.Never);

            _loggerMock.Verify(x => x.LogError(SPublisherEvent.BuildStepExecutionCompletedWithError, _firstStepMock.Object), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.BuildStepWasSkipped, _secondStepMock.Object), Times.Once);
        }
    }
}