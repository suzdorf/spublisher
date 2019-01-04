namespace SPublisher.Configuration
{
    public interface IConfigurationProcessing
    {
        void SetHashingEnabledProperty(ConfigurationModel model);
        void SetRestoreAvailableProperty(ConfigurationModel model);
    }
}