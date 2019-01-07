using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SPublisher.Configuration.BuildSteps;
using SPublisher.Configuration.Exceptions;
using SPublisher.Configuration.JsonConversion;
using SPublisher.Core;
using SPublisher.Core.Log;

namespace SPublisher.Configuration
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        private readonly IDictionary<string, Func<BuildStepModel>> _buildStepModelCreators;
        private readonly IConfigurationValidator _validator;
        private readonly ILogger _logger;
        private readonly IConfigurationProcessing _configurationProcessing;

        public ConfigurationFactory(
            IDictionary<string, Func<BuildStepModel>> buildStepModelCreators,
            IConfigurationValidator validator,
            ILogger logger,
            IConfigurationProcessing configurationProcessing)
        {
            _buildStepModelCreators = buildStepModelCreators;
            _validator = validator;
            _logger = logger;
            _configurationProcessing = configurationProcessing;
        }

        public IConfiguration Get(string json)
        {
            try
            {
                var configuration = JsonConvert.DeserializeObject<ConfigurationModel>(json, new BuildStepsConverter(_buildStepModelCreators));
                _configurationProcessing.SetHashingEnabledProperty(configuration);
                _configurationProcessing.SetRestoreAvailableProperty(configuration);
                _validator.Validate(configuration);
                return configuration;
            }
            catch (JsonReaderException ex)
            {
                _logger.LogError(ex);
                throw new InvalidJsonException();
            }
        }
    }
}
