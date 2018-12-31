using System.Linq;
using SPublisher.Configuration.Models;
using SPublisher.Core;

namespace SPublisher.Configuration
{
    public class RunOptionsFactory : IRunOptionsFactory
    {
        private const string DefaultConfigurationName = "spublisher";

        public IRunOptions Get(string[] args)
        {
            var configurationName =
                args.Any() && !string.IsNullOrEmpty(args[0]) ? args[0] : DefaultConfigurationName;
            return new RunOptionsModel
            {
                ConfigurationFileName = $"{configurationName}.json"
            };
        }
    }
}