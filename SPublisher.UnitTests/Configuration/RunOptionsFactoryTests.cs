using FluentAssertions;
using SPublisher.Configuration;
using SPublisher.Core;
using Xunit;

namespace SPublisher.UnitTests.Configuration
{
    public class RunOptionsFactoryTests
    {
        private readonly IRunOptionsFactory _runOptionsFactory =  new RunOptionsFactory();

        [Theory]
        [InlineData("", "spublisher.json")]
        [InlineData("configurationName", "configurationName.json")]
        public void ShouldGenerateConfigurationFileName(string configurationName, string configurationFileName)
        {
            _runOptionsFactory.Get(new[] {configurationName}).ConfigurationFileName.Should()
                .BeEquivalentTo(configurationFileName);
        }
    }
}