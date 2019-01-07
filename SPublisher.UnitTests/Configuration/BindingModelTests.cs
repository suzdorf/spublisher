using FluentAssertions;
using SPublisher.Configuration.Models;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;
using Xunit;

namespace SPublisher.UnitTests.Configuration
{
    public class BindingModelTests
    {
        private const string CertificateThumbPrint = "CertificateThumbPrint";
        [Theory]
        [InlineData(null, BindingType.Http)]
        [InlineData("", BindingType.Http)]
        [InlineData(CertificateThumbPrint, BindingType.Https)]
        public void BindingTypeParsingTest(string certificate,  BindingType result)
        {
            var model = new BindingModel
            {
                CertificateThumbPrint = certificate
            };
            ((IBinding)model).Type.Should().BeEquivalentTo(result);
        }
    }
}