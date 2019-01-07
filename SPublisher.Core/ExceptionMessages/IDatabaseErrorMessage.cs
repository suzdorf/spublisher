using SPublisher.Core.Log;

namespace SPublisher.Core.ExceptionMessages
{
    public interface IDatabaseErrorMessage : ILogMessage
    {
        string ErrorMessage { get; }
    }
}