using System.Linq;
using Moq;
using SPublisher.Core;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;
using SPublisher.IisManagement;
using Xunit;

namespace SPublisher.UnitTests.IIsManagement
{
    public class BindingsManagerTests
    {
        private readonly Mock<IServerManagerDataProvider> _dataProviderMock =  new Mock<IServerManagerDataProvider>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly IBindingsManager _bindingsManager;

        public BindingsManagerTests()
        {
            _bindingsManager = new BindingsManager(_dataProviderMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ShouldAddBindingIfNotExists()
        {
            const string siteName = "siteName";
            var bindings = new[]
            {
                new Mock<IBinding>().Object,
                new Mock<IBinding>().Object
            };

            _dataProviderMock.Setup(x => x.BindingExists(bindings.First(), siteName)).Returns(true);

            _bindingsManager.Manage(siteName, bindings);
            _dataProviderMock.Verify(x=>x.AddBinding(bindings.First(), siteName), Times.Never);
            _dataProviderMock.Verify(x => x.AddBinding(bindings.Last(), siteName), Times.Once);
            _loggerMock.Verify(x=>x.LogEvent(SPublisherEvent.BindingAdded, bindings.Last()), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.BindingAlreadyExists, bindings.First()), Times.Once);
        }
    }
}