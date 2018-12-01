using Moq;
using SPublisher.Core;
using SPublisher.IisManagement;
using Xunit;

namespace SPublisher.UnitTests.IIsManagement
{
    public class ApplicationCreatorTests
    {
        private readonly Mock<IServerManagerDataProvider> _dataProviderMock = new Mock<IServerManagerDataProvider>();
        private readonly Mock<ILogger> _loggerMock =  new Mock<ILogger>();
        private readonly Mock<IApplication> _applicationMock = new Mock<IApplication>();
        private readonly IApplicationCreator _creator;
        private const string AppPoolName = "AppPoolName";
        private const string SiteName = "SiteName";
        private const string FirstApplicationName = "FirstApplicationName";

        public ApplicationCreatorTests()
        {
            _applicationMock.SetupGet(x => x.Name).Returns(SiteName);
            _creator = new ApplicationCreator(_dataProviderMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ShouldCreateAppPoolForSiteIfNotExists()
        {
            _applicationMock.As<IAppPoolInfo>().SetupGet(x => x.AppPoolName).Returns(AppPoolName);
            _dataProviderMock.Setup(x => x.PoolIsExist(AppPoolName)).Returns(false);
            _creator.Create(_applicationMock.Object);

            _dataProviderMock.Verify(x=>x.CreateAppPool(_applicationMock.Object), Times.Once);
            _loggerMock.Verify(x=>x.LogEvent(SPublisherEvent.ApplicationPoolCreated, _applicationMock.Object), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateAppPoolForSiteIfExists()
        {
            _applicationMock.As<IAppPoolInfo>().SetupGet(x => x.AppPoolName).Returns(AppPoolName);
            _dataProviderMock.Setup(x => x.PoolIsExist(AppPoolName)).Returns(true);
            _creator.Create(_applicationMock.Object);

            _dataProviderMock.Verify(x => x.CreateAppPool(_applicationMock.Object), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ApplicationPoolExists, _applicationMock.Object), Times.Once);
        }

        [Fact]
        public void ShouldCreateSiteIfNotExists()
        {
            _dataProviderMock.Setup(x => x.SiteIsExist(SiteName)).Returns(false);
            _creator.Create(_applicationMock.Object);

            _dataProviderMock.Verify(x => x.CreateSite(_applicationMock.Object), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.SiteCreated, _applicationMock.Object), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateSiteIfExists()
        {
            _dataProviderMock.Setup(x => x.SiteIsExist(SiteName)).Returns(true);
            _creator.Create(_applicationMock.Object);

            _dataProviderMock.Verify(x => x.CreateSite(_applicationMock.Object), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.SiteExists, _applicationMock.Object), Times.Once);
        }

        [Fact]
        public void ShouldCreateNestedApplications()
        {
            var nestedApplicationMock1 = new Mock<IApplication>();
            var nestedApplicationMock2 = new Mock<IApplication>();
            nestedApplicationMock1.SetupGet(x => x.Name).Returns(FirstApplicationName);
            nestedApplicationMock1.SetupGet(x => x.Applications).Returns(new[]
            {
                nestedApplicationMock2.Object
            });
            _applicationMock.SetupGet(x => x.Applications).Returns(new[]
            {
                nestedApplicationMock1.Object
            });

            _dataProviderMock.Setup(x => x.SiteIsExist(SiteName)).Returns(false);

            _creator.Create(_applicationMock.Object);

            _dataProviderMock.Verify(x => x.CreateAppPool(nestedApplicationMock1.Object), Times.Once);
            _dataProviderMock.Verify(x => x.CreateApplication(nestedApplicationMock1.Object, SiteName, "/"), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ApplicationCreated, nestedApplicationMock1.Object), Times.Once);

            _dataProviderMock.Verify(x => x.CreateAppPool(nestedApplicationMock2.Object), Times.Once);
            _dataProviderMock.Verify(x => x.CreateApplication(nestedApplicationMock2.Object, SiteName, $"/{FirstApplicationName}/"), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ApplicationCreated, nestedApplicationMock1.Object), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateApplicationIfExists()
        {
            var nestedApplicationMock = new Mock<IApplication>();
            nestedApplicationMock.SetupGet(x => x.Name).Returns(FirstApplicationName);
            _applicationMock.SetupGet(x => x.Applications).Returns(new[]
            {
                nestedApplicationMock.Object
            });

            _dataProviderMock.Setup(x => x.SiteIsExist(SiteName)).Returns(false);
            _dataProviderMock.Setup(x => x.ApplicationIsExist(SiteName, $"/{FirstApplicationName}")).Returns(true);

            _creator.Create(_applicationMock.Object);

            _dataProviderMock.Verify(x => x.CreateApplication(nestedApplicationMock.Object, SiteName, "/"), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ApplicationExists, nestedApplicationMock.Object), Times.Once);
        }
    }
}