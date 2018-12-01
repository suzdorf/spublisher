using Moq;
using SPublisher.BuildExecutor;
using SPublisher.BuildExecutor.BuildStepExecutors;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using Xunit;

namespace SPublisher.UnitTests.BuildExecutor
{
    public class IisManagementExecutorTests
    {
        private readonly Mock<ISiteCreator> _siteCreatorMock =  new Mock<ISiteCreator>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly Mock<IBuildStep> _buildStepMock = new Mock<IBuildStep>();
        private readonly Mock<IApplication> _firstApplicationMock = new Mock<IApplication>();
        private IApplication[] _applications;
        private readonly IBuildStepExecutor _iisManagementExecutor;

        public IisManagementExecutorTests()
        {
            _iisManagementExecutor = new IisManagementExecutor(_siteCreatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ShouldCallCreateAndLogIfAny()
        {
            _applications = new[]
            {
                _firstApplicationMock.Object
            };
            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Applications).Returns(_applications);

            _iisManagementExecutor.Execute(_buildStepMock.Object);

            _siteCreatorMock.Verify(x=>x.Create(_applications), Times.Once);
            _loggerMock.Verify(x=>x.LogEvent(SPublisherEvent.IisManagementStarted, null), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.IisManagementCompleted, null), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateAndLogIfNone()
        {
            _applications = new IApplication[0];
            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Applications).Returns(_applications);

            _iisManagementExecutor.Execute(_buildStepMock.Object);

            _siteCreatorMock.Verify(x => x.Create(_applications), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.IisManagementStarted, null), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.IisManagementCompleted, null), Times.Never);
        }
    }
}