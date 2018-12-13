using SPublisher.Core;

namespace SPublisher.Configuration
{
    public interface IConfigurationValidator
    {
        void Validate(IConfiguration configuration);
    }
}