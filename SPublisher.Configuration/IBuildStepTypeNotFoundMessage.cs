using SPublisher.Core.Log;

namespace SPublisher.Configuration
{
    public interface IBuildStepTypeNotFoundMessage : ILogMessage
    {
        string Type { get; }
    }
}