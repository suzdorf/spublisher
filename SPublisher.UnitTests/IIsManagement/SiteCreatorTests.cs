using System;
using System.Linq;
using Moq;
using SPublisher.IisManagement;
using Xunit;
using SPublisher.Core;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;

namespace SPublisher.UnitTests.IIsManagement
{
    public class SiteCreatorTests
    {
        private const string FirstSiteName = "FirstSiteName";
        private const string SecondSiteName = "SecondSiteName";
        private readonly Mock<IServerManagerAccessor> _accessorMock = new Mock<IServerManagerAccessor>();
        private readonly Mock<IApplicationCreator> _applicationCreatorMock = new Mock<IApplicationCreator>();
        private readonly Mock<IServerManagerDataProvider> _serverManagerDataProviderMock = new Mock<IServerManagerDataProvider>();
        private readonly Mock<IAppPoolCreator> _appPoolCreatorMock = new Mock<IAppPoolCreator>();
        private readonly Mock<IBindingsManager> _bindingsManagerMock =  new Mock<IBindingsManager>();
        private readonly Mock<IDisposable> _serverManagerMock = new Mock<IDisposable>();
        private readonly Mock<ISite> _firstSiteMock = new Mock<ISite>();
        private readonly Mock<ISite> _secondSiteMock = new Mock<ISite>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly ISite[] _sites;
        private readonly ISiteCreator _siteCreator;

        public SiteCreatorTests()
        {
            _firstSiteMock.SetupGet(x => x.Name).Returns(FirstSiteName);
            _secondSiteMock.SetupGet(x => x.Name).Returns(SecondSiteName);
            _sites = new []{
                _firstSiteMock.Object,
                _secondSiteMock.Object
            };
            _accessorMock.Setup(x => x.ServerManager()).Returns(_serverManagerMock.Object);
            _siteCreator = new SiteCreator(
                _accessorMock.Object,
                _applicationCreatorMock.Object,
                _loggerMock.Object,
                _serverManagerDataProviderMock.Object,
                _appPoolCreatorMock.Object,
                _bindingsManagerMock.Object);
        }

        [Fact]
        public void ShouldCreateServerManagerInstanceCommitChangesAndDispose()
        {
            _siteCreator.Create(_sites);

            _accessorMock.Verify(x=>x.ServerManager(), Times.Once);
            _accessorMock.Verify(x => x.CommitChanges(), Times.Once);
            _serverManagerMock.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public void ShouldCallCreateForEachSiteIfNotExists()
        {
            _serverManagerDataProviderMock.Setup(x => x.SiteIsExist(FirstSiteName)).Returns(false);
            _serverManagerDataProviderMock.Setup(x => x.SiteIsExist(SecondSiteName)).Returns(true);
            _siteCreator.Create(_sites);

            _serverManagerDataProviderMock.Verify(
                x => x.CreateSite(_sites.First(),
                    It.Is<IBinding>(y =>
                        y.HostName == FirstSiteName &&
                        y.IpAddress == Constants.SiteBinding.DefaultIpAddress &&
                        y.Port == Constants.SiteBinding.DefaultPort &&
                        y.Type == BindingType.Http)), Times.Once);

            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.SiteCreated, _sites.First()), Times.Once);

            _serverManagerDataProviderMock.Verify(x => x.CreateSite(_sites.Last(), It.Is<IBinding>(y =>
                y.HostName == SecondSiteName &&
                y.IpAddress == Constants.SiteBinding.DefaultIpAddress &&
                y.Port == Constants.SiteBinding.DefaultPort &&
                y.Type == BindingType.Http)), Times.Never);

            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.SiteCreated, _sites.Last()), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.SiteExists, _sites.Last()), Times.Once);
        }

        [Fact]
        public void ShouldReturnIfApplicationsIsEmptyAndLogEvent()
        {
            _siteCreator.Create(new ISite[0]);

            _accessorMock.Verify(x => x.ServerManager(), Times.Never);
            _accessorMock.Verify(x => x.CommitChanges(), Times.Never);
            _serverManagerMock.Verify(x => x.Dispose(), Times.Never);
            _serverManagerDataProviderMock.Verify(x => x.CreateSite(It.IsAny<ISite>(), null), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ApplicationListIsEmpty, null), Times.Once);
        }

        [Fact]
        public void ShouldCallCreateAppPoolForEachSite()
        {
            _siteCreator.Create(_sites);

            foreach (var site in _sites)
            {
                _appPoolCreatorMock.Verify(x=>x.Create(site), Times.Once);
            }
        }

        [Fact]
        public void ShouldCallCreateApplicationForEachSiteForEachApplicationIfAny()
        {
            var firstSiteApplications = new[]
            {
                new Mock<IApplication>().Object,
                new Mock<IApplication>().Object
            };

            var secondSiteApplications = new[]
            {
                new Mock<IApplication>().Object
            };

            _firstSiteMock.SetupGet(x => x.Applications).Returns(firstSiteApplications);
            _secondSiteMock.SetupGet(x => x.Applications).Returns(secondSiteApplications);

            _siteCreator.Create(_sites);

            foreach (var firstSiteApplication in firstSiteApplications)
            {
                _applicationCreatorMock.Verify(x=>x.Create(firstSiteApplication, FirstSiteName, "/"), Times.Once);
            }

            foreach (var secondSiteApplication in secondSiteApplications)
            {
                _applicationCreatorMock.Verify(x => x.Create(secondSiteApplication, SecondSiteName, "/"), Times.Once);
            }
        }

        [Fact]
        public void ShouldManageBindingsIfAny()
        {
            var bindings = new []
            {
                new Mock<IBinding>().Object,
                new Mock<IBinding>().Object
            };
            _firstSiteMock.SetupGet(x => x.Bindings).Returns(bindings);
            _secondSiteMock.SetupGet(x => x.Bindings).Returns(new IBinding[0]);
            _siteCreator.Create(_sites);

            _serverManagerDataProviderMock.Verify(x => x.CreateSite(_sites.First(), bindings.First()), Times.Once);
            _bindingsManagerMock.Verify(x=>x.Manage(FirstSiteName, bindings), Times.Once);
            _bindingsManagerMock.Verify(x => x.Manage(SecondSiteName, It.IsAny<IBinding[]>()), Times.Never);
        }
    }
}