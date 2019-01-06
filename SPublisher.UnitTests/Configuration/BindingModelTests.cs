using FluentAssertions;
using SPublisher.Configuration.Models;
using SPublisher.Core;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;
using Xunit;

namespace SPublisher.UnitTests.Configuration
{
    public class BindingModelTests
    {
        [Theory]
        [InlineData(null, BindingType.Http)]
        [InlineData("", BindingType.Http)]
        [InlineData(Constants.SiteBinding.Types.Http, BindingType.Http)]
        [InlineData(Constants.SiteBinding.Types.Https, BindingType.Https)]
        [InlineData("random string", BindingType.Invalid)]
        public void BindingTypeParsingTest(string type, BindingType result)
        {
            var model = new BindingModel
            {
                Type = type
            };
            ((IBinding)model).Type.Should().BeEquivalentTo(result);
        }
    }
}