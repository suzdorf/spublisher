using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SPublisher.Configuration.BuildSteps;
using SPublisher.Configuration.JsonConversion;
using SPublisher.Core;

namespace SPublisher.Configuration
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        private readonly IDictionary<string, Func<BuildStepModel>> _buildStepModelCreators;

        public ConfigurationFactory(IDictionary<string, Func<BuildStepModel>> buildStepModelCreators)
        {
            _buildStepModelCreators = buildStepModelCreators;
        }

        public IConfiguration Get(string json)
        {
            return JsonConvert.DeserializeObject<ConfigurationModel>(json, new BuildStepsConverter(_buildStepModelCreators));
        }
    }
}
