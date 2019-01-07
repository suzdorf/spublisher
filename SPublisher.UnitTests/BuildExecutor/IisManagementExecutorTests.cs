using Moq;
using SPublisher.BuildExecutor;
using SPublisher.BuildExecutor.BuildStepExecutors;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;
using SPublisher.Core.Log;
using Xunit;

namespace SPublisher.UnitTests.BuildExecutor
{
    public class IisManagementExecutorTests
    {
        private readonly Mock<ISiteCreator> _siteCreatorMock =  new Mock<ISiteCreator>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly Mock<IBuildStep> _buildStepMock = new Mock<IBuildStep>();
        private readonly Mock<ISite> _firstApplicationMock = new Mock<ISite>();
        private ISite[] _sites;
        private readonly IBuildStepExecutor _iisManagementExecutor;

        public IisManagementExecutorTests()
        {
            _iisManagementExecutor = new IisManagementExecutor(_siteCreatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ShouldCallCreateAndLogIfAny()
        {
            _sites = new[]
            {
                _firstApplicationMock.Object
            };
            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Sites).Returns(_sites);

            _iisManagementExecutor.Execute(_buildStepMock.Object);

            _siteCreatorMock.Verify(x=>x.Create(_sites), Times.Once);
            _loggerMock.Verify(x=>x.LogEvent(SPublisherEvent.IisManagementStarted, null), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.IisManagementCompleted, null), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateAndLogIfNone()
        {
            _sites = new ISite[0];
            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Sites).Returns(_sites);

            _iisManagementExecutor.Execute(_buildStepMock.Object);

            _siteCreatorMock.Verify(x => x.Create(_sites), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.IisManagementStarted, null), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.IisManagementCompleted, null), Times.Never);
        }
    }
}