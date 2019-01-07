using SPublisher.Configuration.BuildSteps;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.Enums;

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

        public void SetRestoreAvailableProperty(ConfigurationModel model)
        {
            foreach (var step in model.BuildSteps)
            {
                if (step is SqlStepModel sqlStep)
                {
                    foreach (var database in sqlStep.Databases)
                    {
                        database.RestoreAvailable = (sqlStep as ISqlStep).ServerType == SqlServerType.MsSql;
                    }
                }
            }
        }
    }
}