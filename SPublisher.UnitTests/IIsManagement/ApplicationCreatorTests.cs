using Moq;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;
using SPublisher.Core.Log;
using SPublisher.IisManagement;
using Xunit;

namespace SPublisher.UnitTests.IIsManagement
{
    public class ApplicationCreatorTests
    {
        private readonly Mock<IServerManagerDataProvider> _dataProviderMock = new Mock<IServerManagerDataProvider>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly Mock<IAppPoolCreator> _appPoolCreatorMock = new Mock<IAppPoolCreator>();
        private readonly IApplicationCreator _creator;
        private const string SiteName = "SiteName";
        private const string FirstApplicationName = "FirstApplicationName";

        public ApplicationCreatorTests()
        {
            _creator = new ApplicationCreator(_dataProviderMock.Object, _loggerMock.Object, _appPoolCreatorMock.Object);
        }

        [Fact]
        public void ShouldCreateNestedApplications()
        {
            var applicationMock = new Mock<IApplication>();
            var nestedApplicationMock = new Mock<IApplication>();
            applicationMock.SetupGet(x => x.Name).Returns(FirstApplicationName);
            applicationMock.SetupGet(x => x.Applications).Returns(new[]
            {
                nestedApplicationMock.Object
            });

            _creator.Create(applicationMock.Object, SiteName);

            _appPoolCreatorMock.Verify(x => x.Create(applicationMock.Object), Times.Once);
            _dataProviderMock.Verify(x => x.CreateApplication(applicationMock.Object, SiteName, "/"), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ApplicationCreated, applicationMock.Object), Times.Once);

            _appPoolCreatorMock.Verify(x => x.Create(nestedApplicationMock.Object), Times.Once);
            _dataProviderMock.Verify(x => x.CreateApplication(nestedApplicationMock.Object, SiteName, $"/{FirstApplicationName}/"), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ApplicationCreated, applicationMock.Object), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateApplicationIfExists()
        {
            var applicationMock = new Mock<IApplication>();
            applicationMock.SetupGet(x => x.Name).Returns(FirstApplicationName);
            
            _dataProviderMock.Setup(x => x.ApplicationIsExist(SiteName, $"/{FirstApplicationName}")).Returns(true);

            _creator.Create(applicationMock.Object, SiteName);

            _dataProviderMock.Verify(x => x.CreateApplication(applicationMock.Object, SiteName, "/"), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ApplicationExists, applicationMock.Object), Times.Once);
        }

        [Fact]
        public void ShouldCreateVirtualDirectoryIfNotExists()
        {
            var virtualDirectoryMock = new Mock<IApplication>();
            virtualDirectoryMock.SetupGet(x => x.Name).Returns(FirstApplicationName);
            virtualDirectoryMock.SetupGet(x => x.IsVirtualDirectory).Returns(true);

            _dataProviderMock.Setup(x => x.SiteIsExist(SiteName)).Returns(false);

            _creator.Create(virtualDirectoryMock.Object, SiteName);

            _dataProviderMock.Verify(x => x.CreateVirtualDirectory(virtualDirectoryMock.Object, SiteName, "/"), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.VirtualDirectoryCreated, virtualDirectoryMock.Object), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateVirtualDirectoryIfExists()
        {
            var virtualDirectoryMock = new Mock<IApplication>();
            virtualDirectoryMock.SetupGet(x => x.Name).Returns(FirstApplicationName);
            virtualDirectoryMock.SetupGet(x => x.IsVirtualDirectory).Returns(true);

            _dataProviderMock.Setup(x => x.SiteIsExist(SiteName)).Returns(false);
            _dataProviderMock.Setup(x => x.VirtualDirectoryIsExist(FirstApplicationName, SiteName, "/")).Returns(true);

            _creator.Create(virtualDirectoryMock.Object, SiteName);

            _dataProviderMock.Verify(x => x.CreateVirtualDirectory(virtualDirectoryMock.Object, SiteName, "/"), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.VirtualDirectoryExists, virtualDirectoryMock.Object), Times.Once);
        }
    }
}