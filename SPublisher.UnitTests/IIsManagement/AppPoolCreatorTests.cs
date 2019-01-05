using Moq;
using SPublisher.Core;
using SPublisher.Core.IisManagement;
using SPublisher.IisManagement;
using Xunit;

namespace SPublisher.UnitTests.IIsManagement
{
    public class AppPoolCreatorTests
    {
        private readonly Mock<IServerManagerDataProvider> _dataProviderMock = new Mock<IServerManagerDataProvider>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private const string AppPoolName = "AppPoolName";
        private readonly Mock<IAppPoolInfo> _appPoolInfoMock = new Mock<IAppPoolInfo>();
        private readonly IAppPoolCreator _creator;

        public AppPoolCreatorTests()
        {
            _creator = new AppPoolCreator(_dataProviderMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ShouldCreateAppPoolForSiteIfNotExists()
        {
            _appPoolInfoMock.SetupGet(x => x.AppPoolName).Returns(AppPoolName);
            _dataProviderMock.Setup(x => x.PoolIsExist(AppPoolName)).Returns(false);
            _creator.Create(_appPoolInfoMock.Object);

            _dataProviderMock.Verify(x => x.CreateAppPool(_appPoolInfoMock.Object), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ApplicationPoolCreated, _appPoolInfoMock.Object), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateAppPoolForSiteIfExists()
        {
            _appPoolInfoMock.SetupGet(x => x.AppPoolName).Returns(AppPoolName);
            _dataProviderMock.Setup(x => x.PoolIsExist(AppPoolName)).Returns(true);
            _creator.Create(_appPoolInfoMock.Object);

            _dataProviderMock.Verify(x => x.CreateAppPool(_appPoolInfoMock.Object), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ApplicationPoolExists, _appPoolInfoMock.Object), Times.Once);
        }
    }
}