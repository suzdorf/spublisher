using Newtonsoft.Json;
using SPublisher.Configuration.JsonConversion;
using SPublisher.Core;

namespace SPublisher.Configuration
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        public IConfiguration Get(string json)
        {
            return JsonConvert.DeserializeObject<ConfigurationModel>(json, new BuildStepsConverter());
        }
    }
}
