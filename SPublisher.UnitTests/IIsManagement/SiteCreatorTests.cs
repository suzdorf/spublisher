using System;
using System.Linq;
using Moq;
using SPublisher.IisManagement;
using Xunit;
using SPublisher.Core;

namespace SPublisher.UnitTests.IIsManagement
{
    public class SiteCreatorTests
    {
        private readonly Mock<IServerManagerAccessor> _accessorMock = new Mock<IServerManagerAccessor>();
        private readonly Mock<IApplicationCreator> _applicationCreatorMock = new Mock<IApplicationCreator>();
        private readonly Mock<IDisposable> _serverManagerMock = new Mock<IDisposable>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly ISiteCreator _siteCreator;
        private readonly IApplication[] _applications = {
            new Mock<IApplication>().Object,
            new Mock<IApplication>().Object
        };

        public SiteCreatorTests()
        {
            _accessorMock.Setup(x => x.ServerManager()).Returns(_serverManagerMock.Object);
            _siteCreator = new SiteCreator(_accessorMock.Object, _applicationCreatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ShouldCreateServerManagerInstanceCommitChangesAndDispose()
        {
            _siteCreator.Create(_applications);

            _accessorMock.Verify(x=>x.ServerManager(), Times.Once);
            _accessorMock.Verify(x => x.CommitChanges(), Times.Once);
            _serverManagerMock.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public void ShouldCallCreateForEachApplication()
        {
            _siteCreator.Create(_applications);

            _applicationCreatorMock.Verify(x=>x.Create(_applications.First()), Times.Once);
            _applicationCreatorMock.Verify(x => x.Create(_applications.Last()), Times.Once);
        }

        [Fact]
        public void ShouldReturnIfApplicationsIsNullAndLogEvent()
        {
            _siteCreator.Create(null);

            _accessorMock.Verify(x => x.ServerManager(), Times.Never);
            _accessorMock.Verify(x => x.CommitChanges(), Times.Never);
            _serverManagerMock.Verify(x => x.Dispose(), Times.Never);
            _applicationCreatorMock.Verify(x => x.Create(It.IsAny<IApplication>()), Times.Never);
            _loggerMock.Verify(x =>x.LogEvent(SPublisherEvent.ApplicationListIsEmpty, null), Times.Once);
        }

        [Fact]
        public void ShouldReturnIfApplicationsIsEmptyAndLogEvent()
        {
            _siteCreator.Create(new IApplication[0]);

            _accessorMock.Verify(x => x.ServerManager(), Times.Never);
            _accessorMock.Verify(x => x.CommitChanges(), Times.Never);
            _serverManagerMock.Verify(x => x.Dispose(), Times.Never);
            _applicationCreatorMock.Verify(x => x.Create(It.IsAny<IApplication>()), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ApplicationListIsEmpty, null), Times.Once);
        }
    }
}