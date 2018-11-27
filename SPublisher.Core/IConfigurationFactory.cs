namespace SPublisher.Core
{
    public interface IConfigurationFactory
    {
        IConfiguration Get(string json);
    }
}
