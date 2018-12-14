using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SPublisher.Configuration.BuildSteps;
using SPublisher.Configuration.Exceptions;
using SPublisher.Configuration.JsonConversion;
using SPublisher.Core;

namespace SPublisher.Configuration
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        private readonly IDictionary<string, Func<BuildStepModel>> _buildStepModelCreators;
        private readonly IConfigurationValidator _validator;

        public ConfigurationFactory(IDictionary<string, Func<BuildStepModel>> buildStepModelCreators, IConfigurationValidator validator)
        {
            _buildStepModelCreators = buildStepModelCreators;
            _validator = validator;
        }

        public IConfiguration Get(string json)
        {
            try
            {
                var configuration = JsonConvert.DeserializeObject<ConfigurationModel>(json, new BuildStepsConverter(_buildStepModelCreators));
                _validator.Validate(configuration);
                return configuration;
            }
            catch (JsonReaderException)
            {
                throw new InvalidJsonException();
            }
        }
    }
}
