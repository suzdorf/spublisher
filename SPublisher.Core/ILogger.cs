namespace SPublisher.Core
{
    public interface ILogger
    {
        void LogEvent(SPublisherEvent sPublisherEvent, ILogMessage logMessage =  null);
        void LogError(SPublisherEvent sPublisherEvent, ILogMessage logMessage = null);
    }
}