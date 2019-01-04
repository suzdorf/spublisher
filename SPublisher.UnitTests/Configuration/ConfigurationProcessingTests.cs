using System.Linq;
using FluentAssertions;
using SPublisher.Configuration;
using SPublisher.Configuration.BuildSteps;
using SPublisher.Configuration.Models;
using SPublisher.Core;
using Xunit;

namespace SPublisher.UnitTests.Configuration
{
    public class ConfigurationProcessingTests
    {
        private readonly IConfigurationProcessing _configurationProcessing = new ConfigurationProcessing();

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(false, true, true, false)]
        [InlineData(true, false, true, false)]
        [InlineData(true, true, false, false)]
        [InlineData(false, true, false, false)]
        [InlineData(true, false, false, false)]

        public void ShouldProcessHashingEnabledProperty(
            bool configurationLevel,
            bool stepLevel,
            bool databaseLevelBefore,
            bool databaseLevelAfter)
        {
            var configuration =  new ConfigurationModel
            {
                HashingEnabled = configurationLevel,
                BuildSteps = new BuildStepModel[]
                {
                    new SqlStepModel
                    {
                        HashingEnabled = stepLevel,
                        Databases = new[]
                        {
                            new DatabaseModel
                            {
                                HashingEnabled = databaseLevelBefore
                            }
                        }
                    } 
                }
            };

            _configurationProcessing.SetHashingEnabledProperty(configuration);

            (configuration.BuildSteps.First() as SqlStepModel).Databases.First().HashingEnabled.Should()
                .Be(databaseLevelAfter);
        }

        [Theory]
        [InlineData("", true)]
        [InlineData(Constants.SqlServerType.MsSql, true)]
        [InlineData(Constants.SqlServerType.MySql, false)]
        [InlineData(Constants.SqlServerType.PostgreSql, false)]
        public void ShouldProcessRestoreAvailableProperty(string serverType, bool restoreAvailable)
        {
            var configuration = new ConfigurationModel
            {
                BuildSteps = new BuildStepModel[]
                {
                    new SqlStepModel
                    {
                        ServerType = serverType,
                        Databases = new[]
                        {
                            new DatabaseModel()
                        }
                    }
                }
            };

            _configurationProcessing.SetRestoreAvailableProperty(configuration);

            (configuration.BuildSteps.First() as SqlStepModel).Databases.First().RestoreAvailable.Should()
                .Be(restoreAvailable);
        }
    }
}