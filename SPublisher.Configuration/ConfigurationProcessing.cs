using System.Linq;
using SPublisher.Configuration.BuildSteps;

namespace SPublisher.Configuration
{
    public class ConfigurationProcessing : IConfigurationProcessing
    {
        public void SetHashingEnabledProperty(ConfigurationModel model)
        {
            foreach (var step in model.BuildSteps)
            {
                if (step is SqlStepModel sqlStep )
                {
                    if (model.HashingEnabled == false || sqlStep.HashingEnabled == false)
                    foreach (var sqlStepDatabase in sqlStep.Databases)
                    {
                        sqlStepDatabase.HashingEnabled = false;
                    }
                }
            }
        }
    }
}