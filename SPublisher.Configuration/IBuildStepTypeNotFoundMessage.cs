using SPublisher.Core;

namespace SPublisher.Configuration
{
    public interface IBuildStepTypeNotFoundMessage : ILogMessage
    {
        string Type { get; }
    }
}