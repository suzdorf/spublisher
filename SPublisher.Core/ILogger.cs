using System;
using SPublisher.Core.Enums;

namespace SPublisher.Core
{
    public interface ILogger : IProcessOutputLogger
    {
        void LogEvent(SPublisherEvent sPublisherEvent, ILogMessage logMessage = null);
        void LogError(SPublisherEvent sPublisherEvent, ILogMessage logMessage = null);
        void LogError(Exception exception);
        void LogValidationError(IValidationInfo info);
    }
}